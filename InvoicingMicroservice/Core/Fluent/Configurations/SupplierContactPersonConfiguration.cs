using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvoicingMicroservice.Core.Fluent.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoicingMicroservice.Core.Fluent.Configurations
{
    public class SupplierContactPersonConfiguration : IEntityTypeConfiguration<SupplierContactPerson>
    {
        public void Configure(EntityTypeBuilder<SupplierContactPerson> modelBuilder)
        {
            modelBuilder.HasKey(d => new { d.Id, d.SupplierId, d.EspId });

            modelBuilder.Property(d => d.Id).ValueGeneratedNever().IsRequired();
            modelBuilder.Property(d => d.SupplierId).IsRequired();
            modelBuilder.Property(d => d.EspId).IsRequired();

            modelBuilder
                .HasOne(scs => scs.Supplier)
                .WithMany(s => s.SupplierContactPersons)
                .HasForeignKey(scs => new { scs.SupplierId, scs.EspId })
                .HasPrincipalKey(s => new { s.Id, s.EspId });

            modelBuilder.Property(d => d.FirstName).HasMaxLength(300).IsRequired();
            modelBuilder.Property(d => d.LastName).HasMaxLength(300).IsRequired();
            modelBuilder.Property(d => d.Email).HasMaxLength(100).IsRequired(false);
            modelBuilder.Property(d => d.PhoneNumber).HasMaxLength(12).IsRequired(false);
            modelBuilder.Property(d => d.Description).HasMaxLength(3000).IsRequired(false);

            modelBuilder.Property(a => a.CreatedEudId).IsRequired();
            modelBuilder.Property(a => a.LastUpdatedEudId).IsRequired(false);
            modelBuilder.Property<DateTime>("CreatedDate").HasDefaultValue<DateTime>(DateTime.Now).IsRequired();
            modelBuilder.Property<DateTime?>("LastUpdatedDate").HasDefaultValue<DateTime?>(null).IsRequired(false);

            modelBuilder.ToTable("SuppliersContactsPersons");
            modelBuilder.Property(d => d.Id).HasColumnName("Id");
            modelBuilder.Property(d => d.SupplierId).HasColumnName("SupplierId");
            modelBuilder.Property(d => d.FirstName).HasColumnName("FirstName");
            modelBuilder.Property(d => d.LastName).HasColumnName("LastName");
            modelBuilder.Property(d => d.Email).HasColumnName("Email");
            modelBuilder.Property(d => d.PhoneNumber).HasColumnName("PhoneNumber");
            modelBuilder.Property(d => d.Description).HasColumnName("Description");
        }
    }
}
