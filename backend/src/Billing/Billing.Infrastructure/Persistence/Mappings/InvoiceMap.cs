using Billing.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Billing.Infrastructure.Persistence.Mappings;

public class InvoiceMap : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.ToTable("Invoices");

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.Number, "IX_Invoice_Number").IsUnique();

        builder.Property(x => x.Number)
            .IsRequired()
            .HasColumnName("Number")
            .HasColumnType("INT")
            .UseIdentityColumn();

        builder.Property(x => x.Status)
            .IsRequired()
            .HasColumnName("Status")
            .HasConversion<string>()
            .HasColumnType("VARCHAR")
            .HasMaxLength(20);

        builder.Property(x => x.CreatedAt)
            .IsRequired()
            .HasColumnName("CreatedAt")
            .HasColumnType("TIMESTAMPTZ");

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("UpdatedAt")
            .HasColumnType("TIMESTAMPTZ");

        builder.HasMany(x => x.Items)
            .WithOne()
            .HasForeignKey(x => x.InvoiceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
