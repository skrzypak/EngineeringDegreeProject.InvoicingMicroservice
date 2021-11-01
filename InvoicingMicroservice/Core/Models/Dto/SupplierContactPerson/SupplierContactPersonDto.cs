using System;
using System.Collections.Generic;

namespace InvoicingMicroservice.Core.Models.Dto.SupplierContactPerson
{
    public class SupplierContactPersonDto<TS> : SupplierContactPersonCoreDto
    {
        public int Id { get; set; }
        public TS Supplier { get; set; }
    }
}
