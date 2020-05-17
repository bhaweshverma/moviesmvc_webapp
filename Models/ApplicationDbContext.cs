using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MoviesMVC.Models;

namespace MoviesMVC.Models
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options)
        {

        }

        public DbSet<User> users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder){
            builder.Entity<User>();

            base.OnModelCreating(builder);
            //builder.Seed();
        }
    }
}