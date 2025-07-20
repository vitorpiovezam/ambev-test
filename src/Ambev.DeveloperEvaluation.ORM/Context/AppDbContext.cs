using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleItem> SaleItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sale>().ToTable("Sales");
            modelBuilder.Entity<SaleItem>().ToTable("SaleItems");

            modelBuilder.Entity<Sale>()
                .HasMany(s => s.Items)
                .WithOne(i => i.Sale)
                .HasForeignKey(i => i.SaleId);

            modelBuilder.Entity<Sale>().Property(p => p.TotalAmount).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<SaleItem>().Property(p => p.UnitPrice).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<SaleItem>().Property(p => p.Discount).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<SaleItem>().Property(p => p.TotalItemAmount).HasColumnType("decimal(18,2)");

            base.OnModelCreating(modelBuilder);
        }
    }
}
