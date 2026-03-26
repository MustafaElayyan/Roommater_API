using Microsoft.EntityFrameworkCore;
using Roommater_API.Models;

namespace Roommater_API.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
}
