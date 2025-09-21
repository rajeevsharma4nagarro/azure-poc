using Microsoft.EntityFrameworkCore;
using SCD.Services.OrderAPI.Models;

namespace SCD.Services.OrderAPI.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { 
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderDetails>()
                .HasOne(cd => cd.OrderHeader)
                .WithMany(cd => cd.OrderDetails)
                .HasForeignKey(cd => cd.OrderHeaderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
    }
}
