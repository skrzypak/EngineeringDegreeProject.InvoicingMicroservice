using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvoicingMicroservice.Core.Fluent.Configurations;
using InvoicingMicroservice.Core.Fluent.Entities;
using Microsoft.EntityFrameworkCore;

namespace InvoicingMicroservice.Core.Fluent
{
    public class MicroserviceContext : DbContext
    {
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentToProduct> DocumentToProducts { get; set; }
        public DbSet<DocumentType> DocumentsTypes { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<SupplierContactPerson> SuppliersContactsPersons { get; set; }
        public DbSet<Product> Products { get; set; }

        public MicroserviceContext(DbContextOptions options) : base(options)
        {
        }

        #region Required
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //foreach (var entity in modelBuilder.Model.GetEntityTypes())
            //{
            //    entity.AddProperty("CreatedDate", typeof(DateTime));
            //    entity.AddProperty("EnterpriseId", typeof(ulong));
            //    entity.AddProperty("DomainUserId", typeof(ulong));
            //}

            modelBuilder.ApplyConfiguration(new DocumentConfiguration());
            modelBuilder.ApplyConfiguration(new DocumentToProductConfiguration());
            modelBuilder.ApplyConfiguration(new DocumentTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new SupplierConfiguration());
            modelBuilder.ApplyConfiguration(new SupplierContactPersonConfiguration());
        }
        #endregion
    }
}
