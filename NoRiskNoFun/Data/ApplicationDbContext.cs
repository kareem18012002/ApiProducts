using Microsoft.EntityFrameworkCore;

namespace NoRiskNoFun.Data
{
    public class ApplicationDbContext : DbContext
    {


        public ApplicationDbContext(DbContextOptions options): base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);

                modelBuilder.Entity<Product>().ToTable("Products");
                modelBuilder.Entity<User>().ToTable("Users");
                modelBuilder.Entity<UserPermissions>().ToTable("UserPermissions").HasKey(x=> new
                {
                    x.UserId,
                    x.PermissionId
                });

        }

       
    }
    }
