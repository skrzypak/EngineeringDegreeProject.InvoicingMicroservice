using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvoicingMicroservice.Core.Models.Dto.SupplierContactPerson;

namespace InvoicingMicroservice.Core.Models.Dto.Supplier
{
    public class SupplierCreateDto : SupplierDto
    {
        public ICollection<SupplierContactPersonBasicDto> SupplierContactPersons { get; set; }
    }
}
