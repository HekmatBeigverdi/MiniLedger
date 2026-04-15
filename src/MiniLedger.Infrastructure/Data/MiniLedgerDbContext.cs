using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MiniLedger.Api.Models;

namespace MiniLedger.Api.Data;

public class MiniLedgerDbContext : IdentityDbContext<AppUser>
{
    public MiniLedgerDbContext(DbContextOptions<MiniLedgerDbContext> options) : base(options)
    {
    }

    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Party> Parties => Set<Party>();
    public DbSet<JournalEntry> JournalEntries => Set<JournalEntry>();
    public DbSet<JournalEntryLine> JournalEntryLines => Set<JournalEntryLine>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Account>(entity =>
        {
            entity.Property(x => x.Code).HasMaxLength(20).IsRequired();
            entity.Property(x => x.Name).HasMaxLength(200).IsRequired();
            entity.HasIndex(x => x.Code).IsUnique();
        });

        modelBuilder.Entity<Party>(entity =>
        {
            entity.Property(x => x.Name).HasMaxLength(200).IsRequired();
            entity.Property(x => x.PhoneNumber).HasMaxLength(20);
        });

        modelBuilder.Entity<JournalEntry>(entity =>
        {
            entity.Property(x => x.EntryNumber).HasMaxLength(50).IsRequired();
            entity.Property(x => x.Description).HasMaxLength(500);
            entity.HasIndex(x => x.EntryNumber).IsUnique();
        });

        modelBuilder.Entity<JournalEntryLine>(entity =>
        {
            entity.Property(x => x.Debit).HasColumnType("decimal(18,2)");
            entity.Property(x => x.Credit).HasColumnType("decimal(18,2)");
            entity.Property(x => x.Description).HasMaxLength(500);

            entity.HasOne(x => x.JournalEntry)
                .WithMany(x => x.Lines)
                .HasForeignKey(x => x.JournalEntryId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.Account)
                .WithMany(x => x.JournalEntryLines)
                .HasForeignKey(x => x.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Party)
                .WithMany(x => x.JournalEntryLines)
                .HasForeignKey(x => x.PartyId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}