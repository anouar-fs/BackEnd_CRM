using BackEnd.Dto;
using BackEnd.Entities;

namespace BackEnd.Services;
public interface IAuthService
{
    public Task CreateUser(User user);
    public Task<(string accessToken, string refreshToken, int userId)> ValidateCredentials(LoginDto logindto);

    public Task<string> validateRefresh(string refreshToken);

    public void removeRefresh(string refreshToken);
}

