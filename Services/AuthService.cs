using BackEnd.Dto;
using BackEnd.Entities;
using BackEnd.Models;
using BackEnd.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace BackEnd.Services;
public class AuthService : IAuthService
{
    private readonly IConfiguration _config;
    private IUserRepository UserRepository { get; set; }

    private static readonly List<RefreshToken> RefreshTokens = new();


    public AuthService(IUserRepository userRepository,IConfiguration config)
    {
        UserRepository = userRepository;
        _config = config;
    }

    public async Task CreateUser(User user)
    {
        var plainPasswored = user.password;
        var hasher = new PasswordHasher<User>();
        string hash = hasher.HashPassword(null, plainPasswored);
        user.password = hash;

        await UserRepository.createUser(user);
    }

    public async Task<string> validateRefresh(string refreshToken)
    {
        var storedToken = RefreshTokens
        .FirstOrDefault(t => t.Token == refreshToken);
        if (storedToken == null || storedToken.ExpiresAt < DateTime.UtcNow) { 
            throw new Exception();
        }
        var user = UserRepository.getUserById(storedToken.UserId).Result;

        var newAccessToken = GenerateJwtToken(user, _config);

        return newAccessToken;
    }

    public void removeRefresh(string refreshToken)
    {
        var storedToken = RefreshTokens.RemoveAll(t => t.Token == refreshToken);
    }

    public async Task<(string accessToken, string refreshToken, int userId)> ValidateCredentials(LoginDto logindto)
    {
        var existingUser = await UserRepository.getUserByUsername(logindto.Username);

        if (existingUser is null) 
        {
            throw new Exception();
        }

        var hashedPasswored = existingUser.password;
        var hasher = new PasswordHasher<User>();

        var result = hasher.VerifyHashedPassword(null, hashedPasswored, logindto.password);

        if (result != PasswordVerificationResult.Success)
        {
            throw new Exception();
        }

        var accessToken = GenerateJwtToken(existingUser, _config);
        var refreshToken = GenerateRefreshToken();

        // save refresh token in memory (later: DB)
        RefreshTokens.Add(new RefreshToken
        {
            Token = refreshToken,
            UserId = existingUser.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        });

        return (accessToken, refreshToken, existingUser.Id);
    }

    private string GenerateJwtToken(User user, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("Jwt");
        var Claims = new[]
        {
                new Claim(ClaimTypes.Name,user.Username),
                new Claim("userId",user.Id.ToString())
            };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: Claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(jwtSettings["ExpireMinutes"]!)),
                signingCredentials: creds
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GenerateRefreshToken()
    {
        return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
    }
}

