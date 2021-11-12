using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvoicingMicroservice.Core.Fluent.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoicingMicroservice.Core.Fluent.Configurations
{
    public class DocumentConfiguration : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> modelBuilder)
        {
            modelBuilder.HasKey(d => new { d.Id, d.SupplierId, d.EspId });

            modelBuilder.Property(d => d.Id).IsRequired();
            modelBuilder.Property(d => d.SupplierId).IsRequired();
            modelBuilder.Property(d => d.DocumentTypeId).IsRequired();
            modelBuilder.Property(d => d.EspId).IsRequired();

            modelBuilder
                .HasOne(d => d.Supplier)
                .WithMany(s => s.Documents)
                .HasForeignKey(d => new { d.SupplierId, d.EspId })
                .HasPrincipalKey(s => new { s.Id, s.EspId });

            modelBuilder
                .HasOne(d => d.DocumentType)
                .WithMany(dt => dt.Documents)
                .HasForeignKey(d => new { d.DocumentTypeId, d.EspId })
                .HasPrincipalKey(dt => new { dt.Id, dt.EspId });

            modelBuilder.Property(d => d.Signature).HasMaxLength(300).IsRequired();
            modelBuilder.Property(d => d.Number).IsRequired();
            modelBuilder.Property(d => d.Description).HasMaxLength(3000).IsRequired();
            modelBuilder.Property(d => d.Date).IsRequired();
            modelBuilder.Property(d => d.State).HasConversion<string>().IsRequired();
            modelBuilder.Property(a => a.CreatedEudId).IsRequired();
            modelBuilder.Property(a => a.LastUpdatedEudId).IsRequired(false);
            modelBuilder.Property<DateTime>("CreatedDate").HasDefaultValue<DateTime>(DateTime.Now).IsRequired();
            modelBuilder.Property<DateTime?>("LastUpdatedDate").HasDefaultValue<DateTime?>(null).IsRequired(false);

            modelBuilder.ToTable("Documents");
            modelBuilder.Property(d => d.Id).HasColumnName("Id");
            modelBuilder.Property(d => d.SupplierId).HasColumnName("SupplierId");
            modelBuilder.Property(d => d.DocumentTypeId).HasColumnName("DocumentTypeId");
            modelBuilder.Property(d => d.Signature).HasColumnName("Signature");
            modelBuilder.Property(d => d.Number).HasColumnName("Number");
            modelBuilder.Property(d => d.Description).HasColumnName("Description");
            modelBuilder.Property(d => d.Date).HasColumnName("Date");
            modelBuilder.Property(d => d.State).HasColumnName("State");
        }
    }
}
