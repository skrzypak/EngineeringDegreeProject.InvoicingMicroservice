using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvoicingMicroservice.Core.Fluent.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoicingMicroservice.Core.Fluent.Configurations
{
    public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> modelBuilder)
        {
            modelBuilder.HasKey(s => new { s.Id, s.EspId });
            modelBuilder.Property(s => s.Id).ValueGeneratedOnAdd().IsRequired();

            modelBuilder.Property(s => s.Nip).HasMaxLength(10).IsRequired();
            modelBuilder.Property(s => s.Code).HasMaxLength(6).IsRequired(false);
            modelBuilder.Property(s => s.CompanyName).HasMaxLength(300).IsRequired();
            modelBuilder.Property(s => s.Krs).HasMaxLength(10).IsRequired(false);
            modelBuilder.Property(s => s.Regon).HasMaxLength(9).IsRequired(false);
            modelBuilder.Property(s => s.Email).HasMaxLength(100).IsRequired(false);
            modelBuilder.Property(s => s.PhoneNumber).HasMaxLength(12).IsRequired(false);
            modelBuilder.Ignore(s => s.Address);
            modelBuilder.Property(s => s.StreetAddress).HasMaxLength(100).IsRequired(false);
            modelBuilder.Property(s => s.City).HasMaxLength(100).IsRequired(false);
            modelBuilder.Property(s => s.State).HasMaxLength(100).IsRequired(false);
            modelBuilder.Property(s => s.PostalCode).HasMaxLength(6).IsRequired(false);
            modelBuilder.Property(s => s.Fax).HasMaxLength(40).IsRequired(false);
            modelBuilder.Property(s => s.Homepage).HasMaxLength(300).IsRequired(false);
            modelBuilder.Property(s => s.Archive).HasDefaultValue<bool>(false).IsRequired();
            modelBuilder.Property(s => s.Description).HasMaxLength(3000).IsRequired(false);

            modelBuilder.Property(a => a.EspId).IsRequired();
            modelBuilder.Property(a => a.CreatedEudId).IsRequired();
            modelBuilder.Property(a => a.LastUpdatedEudId).IsRequired(false);
            modelBuilder.Property<DateTime>("CreatedDate").HasDefaultValue<DateTime>(DateTime.Now).IsRequired();
            modelBuilder.Property<DateTime?>("LastUpdatedDate").HasDefaultValue<DateTime?>(null).IsRequired(false);

            modelBuilder.ToTable("Suppliers");
            modelBuilder.Property(s => s.Id).HasColumnName("Id");
            modelBuilder.Property(s => s.Nip).HasColumnName("NIP");
            modelBuilder.Property(s => s.Code).HasColumnName("Code");
            modelBuilder.Property(s => s.CompanyName).HasColumnName("CompanyName");
            modelBuilder.Property(s => s.Krs).HasColumnName("KRS");
            modelBuilder.Property(s => s.Regon).HasColumnName("REGON");
            modelBuilder.Property(s => s.Email).HasColumnName("Email");
            modelBuilder.Property(s => s.PhoneNumber).HasColumnName("PhoneNumber");
            modelBuilder.Property(s => s.StreetAddress).HasColumnName("StreetAddress");
            modelBuilder.Property(s => s.City).HasColumnName("City");
            modelBuilder.Property(s => s.State).HasColumnName("State");
            modelBuilder.Property(s => s.PostalCode).HasColumnName("PostalCode");
            modelBuilder.Property(s => s.Fax).HasColumnName("Fax");
            modelBuilder.Property(s => s.Homepage).HasColumnName("Homepage");
            modelBuilder.Property(s => s.Archive).HasColumnName("Archive");
            modelBuilder.Property(s => s.Description).HasColumnName("Description");
        }
    }
}
