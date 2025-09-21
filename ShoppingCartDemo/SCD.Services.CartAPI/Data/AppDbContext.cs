using Microsoft.EntityFrameworkCore;
using SCD.Services.CartAPI.Models;

namespace SCD.Services.CartAPI.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { 
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CartDetails>()
                .HasOne(cd => cd.CartHeader)
                .WithMany(ch => ch.CartDetails)
                .HasForeignKey(cd => cd.CartHeaderId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<CartHeader> cartHeaders { get; set; }
        public DbSet<CartDetails> cartDetails { get; set; }
    }
}
