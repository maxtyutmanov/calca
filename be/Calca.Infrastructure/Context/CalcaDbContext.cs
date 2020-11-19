using Calca.Domain.Accounting;
using Calca.Infrastructure.Context.Dto.Accounting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Calca.Infrastructure.Context
{
    public class CalcaDbContext : DbContext
    {
        public CalcaDbContext(DbContextOptions<CalcaDbContext> opts)
            : base(opts)
        {
        }

        public DbSet<LedgerDto> Ledgers { get; set; }

        // TODO: remove?

        public DbSet<MemberDto> LedgerMembers { get; set; }

        public DbSet<OperationDto> LedgerOperations { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            base.OnModelCreating(mb);

            mb.Entity<LedgerDto>()
                .ToTable("Ledgers", "accounting")
                .HasKey(x => x.Id)
                .IsClustered();
            mb.Entity<LedgerDto>()
                .HasMany(x => x.Members)
                .WithOne(x => x.Ledger);
            mb.Entity<LedgerDto>()
                .HasMany(x => x.Operations)
                .WithOne(x => x.Ledger);
            mb.Entity<LedgerDto>()
                .Property(x => x.Version)
                .IsConcurrencyToken();

            mb.Entity<MemberDto>()
                .ToTable("Members", "accounting")
                .HasKey(x => x.Id)
                .IsClustered();
            mb.Entity<MemberDto>()
                .HasIndex(x => new { x.LedgerId, x.UserId })
                .IsUnique();
            mb.Entity<MemberDto>()
                .HasMany(x => x.Operations)
                .WithOne(x => x.Member);

            mb.Entity<OperationDto>()
                .ToTable("Operations", "accounting")
                .HasKey(x => x.Id)
                .IsClustered();
            mb.Entity<OperationDto>()
                .HasMany(x => x.Members)
                .WithOne(x => x.Operation);

            mb.Entity<OperationMemberDto>()
                .ToTable("OperationMembers", "accounting")
                .HasKey(x => new { x.OperationId, x.MemberId, x.Side })
                .IsClustered();
        }
    }
}
