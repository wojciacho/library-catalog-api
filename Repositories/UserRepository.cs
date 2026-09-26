using LibraryCatalog.Data;
using LibraryCatalog.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryCatalog.Repositories;

public class UserRepository : IUserRepository
{

    private readonly LibraryCatalogDbContext _context;

    public UserRepository(LibraryCatalogDbContext context)
    {
        _context = context;
    }

    public async Task<User> AddAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        var userByUsername = await _context.Users.AsNoTracking().Where(u => u.Username == username).FirstOrDefaultAsync();

        if (userByUsername == null)
        {
            return null;
        }

        return userByUsername;
    }
}