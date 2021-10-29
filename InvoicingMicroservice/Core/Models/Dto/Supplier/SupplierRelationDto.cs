using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicingMicroservice.Core.Models.Dto.Supplier
{
    public class SupplierRelationDto<TSCP> : SupplierCoreDto
    {
        public ICollection<TSCP> SupplierContactPersons { get; set; }
    }
}
