using Calca.Domain.Accounting;
using Calca.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Calca.Infrastructure.Context
{
    public class CalcaDbContext : DbContext
    {
        public CalcaDbContext(DbContextOptions<CalcaDbContext> opts)
            : base(opts)
        {
        }

        public DbSet<Ledger> Ledgers { get; set; }

        public DbSet<LedgerOperation> LedgerOperations { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            base.OnModelCreating(mb);

            mb.Entity<Ledger>()
                .ToTable("Ledgers", "accounting");
            mb.Entity<Ledger>()
                .HasMany(x => x.Members);
            mb.Entity<Ledger>()
                .Property(x => x.Version)
                .IsConcurrencyToken();
            mb.Entity<Ledger>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.CreatorId)
                .OnDelete(DeleteBehavior.NoAction);

            mb.Entity<LedgerMember>()
                .ToTable("LedgerMembers", "accounting")
                .HasKey(x => new { x.LedgerId, x.UserId });
            mb.Entity<LedgerMember>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            mb.Entity<LedgerOperation>()
                .ToTable("LedgerOperations", "accounting");
            mb.Entity<LedgerOperation>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.CreatorId)
                .OnDelete(DeleteBehavior.NoAction);
            mb.Entity<LedgerOperation>()
                .HasOne<Ledger>()
                .WithMany()
                .HasForeignKey(x => x.LedgerId);

            mb.Entity<OperationMember>()
                .ToTable("OperationMembers", "accounting")
                .HasKey(x => new { x.OperationId, x.UserId, x.Side });
            mb.Entity<OperationMember>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            mb.Entity<OperationMember>()
                .HasOne<LedgerOperation>()
                .WithMany(x => x.Members)
                .HasForeignKey(x => x.OperationId);

            mb.Entity<User>()
                .ToTable("Users", "auth");
        }
    }
}
