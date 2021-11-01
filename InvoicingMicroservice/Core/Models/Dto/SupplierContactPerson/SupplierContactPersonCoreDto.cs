using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using InvoicingMicroservice.Core.Interfaces.ViewModel;

namespace InvoicingMicroservice.Core.Models.Dto.SupplierContactPerson
{
    public class SupplierContactPersonCoreDto : IDto
    {
        [MinLength(1), MaxLength(300)]
        public string FirstName { get; set; }
        [MinLength(1), MaxLength(300)]
        public string LastName { get; set; }
        [MaxLength(100)]
        public string Email { get; set; }
        [MaxLength(12)]
        public string PhoneNumber { get; set; }
        [MaxLength(3000)]
        public string Description { get; set; }
    }
}
