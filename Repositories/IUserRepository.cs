using LibraryCatalog.Models;

namespace LibraryCatalog.Repositories;

public interface IUserRepository
{
    public Task<User?> GetByUsernameAsync(string username);
    public Task<User> AddAsync(User user);
}