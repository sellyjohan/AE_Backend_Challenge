using AE_Backend.Model;
using Microsoft.EntityFrameworkCore;

namespace AE_Backend.db
{
    public class MyDbContext : DbContext, IMyDbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }

        public MyDbContext()
        {
            // You can optionally initialize any default settings here
        }
        // DbSet properties for your entities
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<Ship> Ships { get; set; }
        public virtual DbSet<UserShip> UserShips { get; set; }
        public virtual DbSet<Port> Ports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure your model relationships here
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            modelBuilder.Entity<UserShip>()
                .HasKey(us => new { us.UserId, us.ShipId });

            modelBuilder.Entity<UserShip>()
                .HasOne(us => us.User)
                .WithMany(u => u.UserShips)
                .HasForeignKey(us => us.UserId);

            modelBuilder.Entity<UserShip>()
                .HasOne(us => us.Ship)
                .WithMany(s => s.UserShips)
                .HasForeignKey(us => us.ShipId);

            // Additional configurations for your model

            base.OnModelCreating(modelBuilder);
        }
    }
}