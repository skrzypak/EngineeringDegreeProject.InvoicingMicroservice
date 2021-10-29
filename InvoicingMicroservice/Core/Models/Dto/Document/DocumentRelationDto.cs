using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicingMicroservice.Core.Models.Dto.Document
{
    public class DocumentRelationDto<TS, TDT, TDP> : DocumentCoreDto<TDT>
    {
        public TS Supplier { get; set; }
        public ICollection<TDP> Products { get; set; }
    }
}
