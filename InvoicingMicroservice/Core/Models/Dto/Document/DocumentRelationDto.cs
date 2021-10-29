using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicingMicroservice.Core.Models.Dto.Document
{
    public class DocumentRelationDto<TDT, TDP> : DocumentCoreDto<TDT>
    {
        public int SupplierId { get; set; }
        public ICollection<TDP> Products { get; set; }
    }
}
