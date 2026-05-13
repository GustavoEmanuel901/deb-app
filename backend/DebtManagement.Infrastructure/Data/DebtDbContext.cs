using Microsoft.EntityFrameworkCore;
using DebtManagement.Domain.Entities;

namespace DebtManagement.Infrastructure.Data
{
    public class DebtDbContext : DbContext
    {
        public DebtDbContext(DbContextOptions<DebtDbContext> options) : base(options)
        {
        }

        public DbSet<Debt> Debts { get; set; }
        public DbSet<Installment> Installments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Debt>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TitleNumber)
                    .IsRequired()
                    .HasMaxLength(50);
                
                entity.Property(e => e.DebtorName)
                    .IsRequired()
                    .HasMaxLength(200);
                
                entity.Property(e => e.DebtorCpf)
                    .IsRequired()
                    .HasMaxLength(14);
                
                entity.Property(e => e.InterestRate)
                    .HasPrecision(5, 2);
                
                entity.Property(e => e.FineRate)
                    .HasPrecision(5, 2);
                
                entity.Property(e => e.CreatedAt)
                    .IsRequired();

                entity.HasMany(e => e.Installments)
                    .WithOne()
                    .HasForeignKey(i => i.DebtId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Navigation(e => e.Installments)
                    .UsePropertyAccessMode(PropertyAccessMode.Field);
            });

            modelBuilder.Entity<Installment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Number)
                    .IsRequired();
                
                entity.Property(e => e.DueDate)
                    .IsRequired();
                
                entity.Property(e => e.Amount)
                    .HasPrecision(18, 2);
                
                entity.Property(e => e.DebtId)
                    .IsRequired();
            });
        }
    }
}