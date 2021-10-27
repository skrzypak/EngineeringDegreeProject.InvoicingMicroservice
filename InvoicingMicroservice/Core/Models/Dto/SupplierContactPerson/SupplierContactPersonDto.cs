using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicingMicroservice.Core.Models.Dto.SupplierContactPerson
{
    public class SupplierContactPersonDto : SupplierContactPersonBasicDto
    {
        public int Id { get; set; }
        public int SupplierId { get; set; }
    }
}
