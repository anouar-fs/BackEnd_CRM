using BackEnd.Entities;

namespace BackEnd.Repositories;
public interface IUserRepository
{
    public Task createUser(User user);
    public Task<User> getUserByUsername(string username);
    public Task<User> getUserById(int id);
}

