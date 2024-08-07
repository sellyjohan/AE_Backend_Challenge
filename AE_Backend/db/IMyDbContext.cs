using AE_Backend.Model;
using Microsoft.EntityFrameworkCore;

namespace AE_Backend.db
{
    public interface IMyDbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<UserRole> UserRoles { get; set; }
        DbSet<Ship> Ships { get; set; }
        DbSet<UserShip> UserShips { get; set; }
        DbSet<Port> Ports { get; set; }
    }
}
