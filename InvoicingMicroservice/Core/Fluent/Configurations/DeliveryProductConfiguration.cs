using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvoicingMicroservice.Core.Fluent.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoicingMicroservice.Core.Fluent.Configurations
{
    public class DeliveryProductConfiguration : IEntityTypeConfiguration<DeliveryProduct>
    {
        public void Configure(EntityTypeBuilder<DeliveryProduct> modelBuilder)
        {
            modelBuilder.HasKey(d => d.Id);
            modelBuilder.Property(d => d.Id).IsRequired();

            modelBuilder.Property(d => d.ProductId).IsRequired();
            modelBuilder.Property(d => d.DeliveryDocumentId).IsRequired();

            modelBuilder.Property(d => d.Quantity).IsRequired();
            modelBuilder.Property(d => d.UnitNetPrice).HasPrecision(13, 3).IsRequired();
            modelBuilder.Property(d => d.PercentageVat).HasPrecision(4, 2).IsRequired();
            modelBuilder.Property(d => d.NetValue).HasPrecision(13, 3).IsRequired();
            modelBuilder.Property(d => d.VatValue).HasPrecision(13, 3).IsRequired();
            modelBuilder.Property(d => d.GrossValue).HasPrecision(13, 3).IsRequired();
            modelBuilder.Property(d => d.Transfered).HasDefaultValue<bool>(false).IsRequired();

            modelBuilder.ToTable("DeliveriesProducts");
            modelBuilder.Property(d => d.Id).HasColumnName("Id");
            modelBuilder.Property(d => d.ProductId).HasColumnName("ProductId");
            modelBuilder.Property(d => d.DeliveryDocumentId).HasColumnName("DeliveryDocumentId");
            modelBuilder.Property(d => d.Quantity).HasColumnName("Quantity");
            modelBuilder.Property(d => d.UnitNetPrice).HasColumnName("UnitNetPrice");
            modelBuilder.Property(d => d.PercentageVat).HasColumnName("PercentageVat");
            modelBuilder.Property(d => d.NetValue).HasColumnName("NetValue");
            modelBuilder.Property(d => d.VatValue).HasColumnName("VatValue");
            modelBuilder.Property(d => d.GrossValue).HasColumnName("GrossValue");
            modelBuilder.Property(d => d.Transfered).HasColumnName("Transfered");
        }
    }
}
