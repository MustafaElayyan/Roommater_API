using Roommater_API.Models;

namespace Roommater_API.Services;

public interface IUserService
{
    string NormalizeEmail(string email);
    User? GetByEmail(string email);
    User? GetById(Guid id);
    User Create(string email, string password);
    bool VerifyPassword(User user, string password);
}
