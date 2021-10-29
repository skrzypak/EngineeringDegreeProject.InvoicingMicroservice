using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicingMicroservice.Core.Models.Dto.SupplierContactPerson
{
    public class SupplierContactPersonRelationDto<TS> : SupplierContactPersonCoreDto
    {
        public TS Supplier { get; set; }
    }
}
