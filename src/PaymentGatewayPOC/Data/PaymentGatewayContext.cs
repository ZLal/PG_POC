using Microsoft.EntityFrameworkCore;
using PaymentGatewayPOC.Models;

namespace PaymentGatewayPOC.Data;

public class PaymentGatewayContext : DbContext
{
    public PaymentGatewayContext(DbContextOptions<PaymentGatewayContext> options)
        : base(options)
    {
    }

    public DbSet<Organization> Organizations { get; set; } = null!;
    public DbSet<Application> Applications { get; set; } = null!;
    public DbSet<Gateway> Gateways { get; set; } = null!;
    public DbSet<Transaction> Transactions { get; set; } = null!;
    public DbSet<TransactionDetail> TransactionDetails { get; set; } = null!;
    public DbSet<ErrorLog> ErrorLogs { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure relationships
        modelBuilder.Entity<Application>()
            .HasOne(a => a.Organization)
            .WithMany(o => o.Applications)
            .HasForeignKey(a => a.OrganizationId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.Application)
            .WithMany(a => a.Transactions)
            .HasForeignKey(t => t.ApplicationId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.Gateway)
            .WithMany(g => g.Transactions)
            .HasForeignKey(t => t.GatewayId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TransactionDetail>()
            .HasOne(td => td.Transaction)
            .WithMany(t => t.TransactionDetails)
            .HasForeignKey(td => td.TransactionId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<ErrorLog>()
            .HasOne(el => el.Transaction)
            .WithMany(t => t.ErrorLogs)
            .HasForeignKey(el => el.TransactionId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}