using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using InvoicingMicroservice.Core.Interfaces.ViewModel;

namespace InvoicingMicroservice.Core.Models.Dto.Supplier
{
    public class SupplierCoreDto : IDto
    {
        [Required, MinLength(10), MaxLength(10)]
        public string Nip { get; set; }
        [MaxLength(6)]
        public string Code { get; set; }
        [MinLength(1), MaxLength(300)]
        public string CompanyName { get; set; }
        [MaxLength(10)]
        public string Krs { get; set; }
        [MaxLength(9)]
        public string Regon { get; set; }
        public string Email { get; set; }
        [MaxLength(12)]
        public string PhoneNumber { get; set; }
        [MaxLength(100)]
        public string StreetAddress { get; set; }
        [MaxLength(6)]
        public string PostalCode { get; set; }
        [MaxLength(100)]
        public string City { get; set; }
        [MaxLength(100)]
        public string State { get; set; }
        [MaxLength(40)]
        public string Fax { get; set; }
        public string Homepage { get; set; }
        public bool Archive { get; set; } = false;
        [MaxLength(3000)]
        public string Description { get; set; }
    }
}
