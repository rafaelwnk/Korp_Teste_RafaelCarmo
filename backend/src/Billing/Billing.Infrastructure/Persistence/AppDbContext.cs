using Billing.Domain.Entities;
using Billing.Infrastructure.Persistence.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Billing.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<InvoiceItem> InvoiceItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new InvoiceMap());
        modelBuilder.ApplyConfiguration(new InvoiceItemMap());
    }
}
