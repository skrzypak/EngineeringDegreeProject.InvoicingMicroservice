using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvoicingMicroservice.Core.Fluent.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoicingMicroservice.Core.Fluent.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> modelBuilder)
        {
            modelBuilder.HasKey(p => new { p.Id, p.EspId });
            modelBuilder.Property(p => p.Id).ValueGeneratedOnAdd().IsRequired();

            modelBuilder.Property(p => p.Code).HasMaxLength(6).IsRequired(false);
            modelBuilder.Property(p => p.Name).HasMaxLength(300).IsRequired();
            modelBuilder.Property(p => p.Unit).HasConversion<string>().HasMaxLength(10).IsRequired();
            modelBuilder.Property(p => p.Description).HasMaxLength(3000).IsRequired(false);

            modelBuilder.Property(a => a.EspId).IsRequired();
            modelBuilder.Property(a => a.CreatedEudId).IsRequired();
            modelBuilder.Property(a => a.LastUpdatedEudId).IsRequired(false);
            modelBuilder.Property<DateTime>("CreatedDate").HasDefaultValue<DateTime>(DateTime.Now).IsRequired();
            modelBuilder.Property<DateTime?>("LastUpdatedDate").HasDefaultValue<DateTime?>(null).IsRequired(false);

            modelBuilder.ToTable("Products");
            modelBuilder.Property(p => p.Id).HasColumnName("Id");
            modelBuilder.Property(p => p.Code).HasColumnName("Code");
            modelBuilder.Property(p => p.Name).HasColumnName("Name");
            modelBuilder.Property(p => p.Unit).HasColumnName("Unit");
            modelBuilder.Property(p => p.Description).HasColumnName("Description");
        }
    }
}
