using System.Collections.Concurrent;
using Roommater_API.Models;

namespace Roommater_API.Services;

public class InMemoryUserService : IUserService
{
    private readonly ConcurrentDictionary<string, User> _usersByEmail = new(StringComparer.OrdinalIgnoreCase);
    private readonly ConcurrentDictionary<Guid, User> _usersById = new();

    public string NormalizeEmail(string email)
    {
        return email.Trim().ToLowerInvariant();
    }

    public User? GetByEmail(string email)
    {
        _usersByEmail.TryGetValue(email, out var user);
        return user;
    }

    public User? GetById(Guid id)
    {
        _usersById.TryGetValue(id, out var user);
        return user;
    }

    public User Create(string email, string password)
    {
        var normalizedEmail = NormalizeEmail(email);
        var user = new User
        {
            Email = normalizedEmail,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
        };

        if (!_usersByEmail.TryAdd(normalizedEmail, user))
        {
            throw new InvalidOperationException("User with this email already exists.");
        }

        if (!_usersById.TryAdd(user.Id, user))
        {
            _usersByEmail.TryRemove(normalizedEmail, out _);
            throw new InvalidOperationException("Failed to store user.");
        }
        return user;
    }

    public bool VerifyPassword(User user, string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
    }
}
