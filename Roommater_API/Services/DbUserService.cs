using Microsoft.EntityFrameworkCore;
using Roommater_API.Data;
using Roommater_API.Models;

namespace Roommater_API.Services;

public class DbUserService : IUserService
{
    private readonly ApplicationDbContext _dbContext;

    public DbUserService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public string NormalizeEmail(string email)
    {
        return email.Trim().ToLowerInvariant();
    }

    public User? GetByEmail(string email)
    {
        return _dbContext.Users
            .Include(u => u.Profile)
            .FirstOrDefault(u => u.Email == email);
    }

    public User? GetById(Guid id)
    {
        return _dbContext.Users
            .Include(u => u.Profile)
            .FirstOrDefault(u => u.Id == id);
    }

    public User Create(string email, string password)
    {
        var normalizedEmail = NormalizeEmail(email);
        var exists = _dbContext.Users.Any(u => u.Email == normalizedEmail);
        if (exists)
        {
            throw new InvalidOperationException("User with this email already exists.");
        }

        var user = new User
        {
            Email = normalizedEmail,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
        };

        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();

        return user;
    }

    public bool VerifyPassword(User user, string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
    }
}
