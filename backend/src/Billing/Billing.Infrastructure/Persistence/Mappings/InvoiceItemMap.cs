using Billing.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Billing.Infrastructure.Persistence.Mappings;

public class InvoiceItemMap : IEntityTypeConfiguration<InvoiceItem>
{
    public void Configure(EntityTypeBuilder<InvoiceItem> builder)
    {
        builder.ToTable("InvoiceItems");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ProductId)
            .IsRequired()
            .HasColumnName("ProductId");

        builder.Property(x => x.ProductCode)
            .IsRequired()
            .HasColumnName("ProductCode")
            .HasColumnType("VARCHAR")
            .HasMaxLength(30);

        builder.Property(x => x.Quantity)
            .IsRequired()
            .HasColumnName("Quantity")
            .HasColumnType("INT");
    }
}
