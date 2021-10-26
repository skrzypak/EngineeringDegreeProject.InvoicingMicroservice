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
            modelBuilder.HasKey(d => d.Id);
            modelBuilder.Property(d => d.Id).IsRequired();

            modelBuilder.Property(d => d.Code).IsRequired();
            modelBuilder.Property(d => d.Name).IsRequired();
            modelBuilder.Property(d => d.Description).IsRequired();

            modelBuilder.ToTable("DocumentTypes");
            modelBuilder.Property(d => d.Id).HasColumnName("Id");
            modelBuilder.Property(d => d.Code).HasColumnName("Code");
            modelBuilder.Property(d => d.Name).HasColumnName("Name");
            modelBuilder.Property(d => d.Description).HasColumnName("Description");
        }
    }
}
