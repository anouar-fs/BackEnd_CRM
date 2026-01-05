using Azure.Core;
using BackEnd.Dto;
using BackEnd.Entities;
using BackEnd.Repositories;
using BackEnd.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace BackEnd.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;

        public AuthController(IConfiguration config,IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> AddUser([FromBody] User user,AppDbContext dbContext)
        {
            try
            {
                await _authService.CreateUser(user);
            }
            catch (Exception ex) 
            {
                return BadRequest(new { message = ex.Message });
            }
                
            return Ok(new { message = "User added successfully" });
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> AddUser([FromBody] LoginDto logindto)
        {

            try
            {
                var (accessToken, refreshToken, userId) = await _authService.ValidateCredentials(logindto);

                Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddDays(7)
                });

                return Ok(new { accessToken });
            }
            catch (Exception ex) 
            {
                return BadRequest(new { message = "Invalid Credentials" });
            } 
        }

        [AllowAnonymous]
        [HttpPost("Logout")]
        public async Task<IActionResult> LogOut()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            _authService.removeRefresh(refreshToken);
            return Ok(new { accessToken = refreshToken });
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult>  Refresh()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (refreshToken == null)
                return Unauthorized("No refresh token found");

            var newAccessToken = await _authService.validateRefresh(refreshToken);

            return Ok(new { accessToken = newAccessToken });
        }


        [HttpGet]
        public async Task<User> GetUsers(AppDbContext dbContext)
        {
            var hasher = new PasswordHasher<User>();

            var user = await dbContext.Users.FirstOrDefaultAsync();

            return user;
        }

    }
}
