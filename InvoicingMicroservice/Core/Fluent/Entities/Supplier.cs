using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicingMicroservice.Core.Fluent.Entities
{
    public class Supplier : IEntity
    {
        public int Id { get; set; }
        public string Nip { get; set; }
        public string Code { get; set; }
        public string CompanyName { get; set; }
        public string Krs { get; set; }
        public string Regon { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address => $"{CompanyName} {StreetAddress} {PostalCode} {City}";
        public string StreetAddress { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Fax { get; set; }
        public string Homepage { get; set; }
        public bool Archive { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<SupplierContactPerson> SupplierContactPersons { get; set; }
    }
}
