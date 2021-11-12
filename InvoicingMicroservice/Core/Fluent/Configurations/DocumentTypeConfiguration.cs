using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvoicingMicroservice.Core.Fluent.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoicingMicroservice.Core.Fluent.Configurations
{
    public class DocumentTypeConfiguration : IEntityTypeConfiguration<DocumentType>
    {
        public void Configure(EntityTypeBuilder<DocumentType> modelBuilder)
        {
            modelBuilder.HasKey(d => new { d.Id, d.EspId });
            modelBuilder.Property(d => d.Id).IsRequired();

            modelBuilder.Property(d => d.Code).IsRequired();
            modelBuilder.Property(d => d.Name).IsRequired();
            modelBuilder.Property(d => d.Description).IsRequired();

            modelBuilder.Property(a => a.EspId).IsRequired();
            modelBuilder.Property(a => a.CreatedEudId).IsRequired();
            modelBuilder.Property(a => a.LastUpdatedEudId).IsRequired(false);
            modelBuilder.Property<DateTime>("CreatedDate").HasDefaultValue<DateTime>(DateTime.Now).IsRequired();
            modelBuilder.Property<DateTime?>("LastUpdatedDate").HasDefaultValue<DateTime?>(null).IsRequired(false);

            modelBuilder.ToTable("DocumentTypes");
            modelBuilder.Property(d => d.Id).HasColumnName("Id");
            modelBuilder.Property(d => d.Code).HasColumnName("Code");
            modelBuilder.Property(d => d.Name).HasColumnName("Name");
            modelBuilder.Property(d => d.Description).HasColumnName("Description");
        }
    }
}
