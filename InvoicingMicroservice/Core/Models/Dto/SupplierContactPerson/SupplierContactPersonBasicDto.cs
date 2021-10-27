using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using InvoicingMicroservice.Core.Interfaces.ViewModel;

namespace InvoicingMicroservice.Core.Models.Dto.SupplierContactPerson
{
    public class SupplierContactPersonBasicDto : IDto
    {
        [MinLength(1), MaxLength(300)]
        public string FirstName { get; set; }
        [MinLength(1), MaxLength(300)]
        public string LastName { get; set; }
        //[EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }
        //[Phone]
        [MaxLength(12)]
        public string PhoneNumber { get; set; }
        [MaxLength(3000)]
        public string Description { get; set; }
    }
}
