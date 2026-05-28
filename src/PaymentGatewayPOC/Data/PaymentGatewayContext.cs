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
    public DbSet<Client> Clients { get; set; } = null!;
    public DbSet<Gateway> Gateways { get; set; } = null!;
    public DbSet<ApplicationGateway> ApplicationGateways { get; set; } = null!;
    public DbSet<Transaction> Transactions { get; set; } = null!;
    public DbSet<TransactionDetail> TransactionDetails { get; set; } = null!;
    public DbSet<ErrorLog> ErrorLogs { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Organization>(entity =>
        {
            entity.HasKey(o => o.OrganizationId);
            entity.Property(o => o.Name)
                .IsRequired()
                .HasMaxLength(255);
            
            entity.HasIndex(o => o.Name)
                .IsUnique();
        });

        modelBuilder.Entity<Application>(entity =>
        {
            entity.HasKey(a => a.ApplicationId);
            entity.Property(a => a.OrganizationId).IsRequired();
            entity.Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(50);
            entity.HasOne(a => a.Organization)
                .WithMany(o => o.Applications)
                .HasForeignKey(a => a.OrganizationId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasIndex(a => new { a.OrganizationId, a.Name })
                .IsUnique();
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(c => c.ClientId);
            entity.Property(c => c.OrganizationId).IsRequired();
            entity.Property(c => c.ApplicationId).IsRequired();
            entity.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(c => c.SecretKey)
                .IsRequired();
            entity.HasOne(c => c.Application)
                .WithMany(a => a.Clients)
                .HasForeignKey(c => c.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasIndex(c => new { c.ApplicationId, c.Name })
                .IsUnique();
        });

        modelBuilder.Entity<Gateway>(entity =>
        {
            entity.HasKey(g => g.GatewayId);
            entity.Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(g => g.Status)
                .IsRequired();

            entity.HasIndex(g => g.Name)
                .IsUnique();
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(t => t.TransactionId);
            entity.Property(t => t.ApplicationId).IsRequired();
            entity.Property(t => t.GatewayId).IsRequired();
            entity.Property(t => t.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
            entity.Property(t => t.Status)
                .IsRequired();
            entity.HasOne(t => t.Application)
                .WithMany(a => a.Transactions)
                .HasForeignKey(t => t.ApplicationId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(t => t.Gateway)
                .WithMany(g => g.Transactions)
                .HasForeignKey(t => t.GatewayId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ApplicationGateway>(entity =>
        {
            entity.HasKey(ag => new { ag.ApplicationId, ag.GatewayId });
            entity.HasOne(ag => ag.Application)
                .WithMany(a => a.ApplicationGateways)
                .HasForeignKey(ag => ag.ApplicationId)
                .OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(ag => ag.Gateway)
                .WithMany(g => g.ApplicationGateways)
                .HasForeignKey(ag => ag.GatewayId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<TransactionDetail>(entity =>
        {
            entity.HasKey(td => td.TransactionDetailId);
            entity.Property(td => td.Status)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(td => td.Message)
                .HasMaxLength(1000);
            entity.HasOne(td => td.Transaction)
                .WithMany(t => t.TransactionDetails)
                .HasForeignKey(td => td.TransactionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ErrorLog>(entity =>
        {
            entity.HasKey(el => el.LogId);
            entity.Property(el => el.ErrorMessage)
                .IsRequired();
        });
    }
}