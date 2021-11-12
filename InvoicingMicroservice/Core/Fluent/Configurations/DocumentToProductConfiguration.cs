using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvoicingMicroservice.Core.Fluent.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoicingMicroservice.Core.Fluent.Configurations
{
    public class DocumentToProductConfiguration : IEntityTypeConfiguration<DocumentToProduct>
    {
        public void Configure(EntityTypeBuilder<DocumentToProduct> modelBuilder)
        {
            modelBuilder.HasKey(d => new { d.Id ,d.DocumentId, d.ProductId, d.EspId });
            modelBuilder.Property(d => d.Id).ValueGeneratedNever().IsRequired();

            modelBuilder.Property(d => d.ProductId).IsRequired();
            modelBuilder.Property(d => d.DocumentId).IsRequired();
            modelBuilder.Property(a => a.EspId).IsRequired();

            modelBuilder
                .HasOne(d2p => d2p.Product)
                .WithMany(p => p.DocumentsToProducts)
                .HasForeignKey(d2p => new { d2p.ProductId, d2p.EspId })
                .HasPrincipalKey(p => new { p.Id, p.EspId });

            modelBuilder
               .HasOne(d2p => d2p.Document)
               .WithMany(d => d.DocumentsToProducts)
               .HasForeignKey(d2p => new { d2p.DocumentId, d2p.EspId })
               .HasPrincipalKey(d => new { d.Id, d.EspId });

            modelBuilder.Property(d => d.Quantity).IsRequired();
            modelBuilder.Property(d => d.UnitMeasureValue).IsRequired();
            modelBuilder.Property(d => d.UnitNetPrice).HasPrecision(13, 3).IsRequired();
            modelBuilder.Property(d => d.PercentageVat).HasPrecision(4, 2).IsRequired();
            modelBuilder.Property(d => d.NetValue).HasPrecision(13, 3).IsRequired();
            modelBuilder.Property(d => d.VatValue).HasPrecision(13, 3).IsRequired();
            modelBuilder.Property(d => d.GrossValue).HasPrecision(13, 3).IsRequired();
            modelBuilder.Property(d => d.Transfered).HasDefaultValue<bool>(false).IsRequired();
            modelBuilder.Property(d => d.ExpirationDate).IsRequired(false);

            modelBuilder.Property(a => a.CreatedEudId).IsRequired();
            modelBuilder.Property(a => a.LastUpdatedEudId).IsRequired(false);
            modelBuilder.Property<DateTime>("CreatedDate").HasDefaultValue<DateTime>(DateTime.Now).IsRequired();
            modelBuilder.Property<DateTime?>("LastUpdatedDate").HasDefaultValue<DateTime?>(null).IsRequired(false);

            modelBuilder.ToTable("DocumentsToProducts");
            modelBuilder.Property(d => d.Id).HasColumnName("Id");
            modelBuilder.Property(d => d.ProductId).HasColumnName("ProductId");
            modelBuilder.Property(d => d.DocumentId).HasColumnName("DocumentId");
            modelBuilder.Property(d => d.Quantity).HasColumnName("Quantity");
            modelBuilder.Property(d => d.UnitMeasureValue).HasColumnName("UnitMeasureValue");
            modelBuilder.Property(d => d.UnitNetPrice).HasColumnName("UnitNetPrice");
            modelBuilder.Property(d => d.PercentageVat).HasColumnName("PercentageVat");
            modelBuilder.Property(d => d.NetValue).HasColumnName("NetValue");
            modelBuilder.Property(d => d.VatValue).HasColumnName("VatValue");
            modelBuilder.Property(d => d.GrossValue).HasColumnName("GrossValue");
            modelBuilder.Property(d => d.Transfered).HasColumnName("Transfered");
            modelBuilder.Property(d => d.ExpirationDate).HasColumnName("ExpirationDate");
        }
    }
}
