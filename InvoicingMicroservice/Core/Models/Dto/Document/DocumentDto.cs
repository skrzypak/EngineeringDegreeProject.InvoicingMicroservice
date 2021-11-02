using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicingMicroservice.Core.Models.Dto.Document
{
    public class DocumentDto<TDT, TDP, TDS> : DocumentCoreDto<TDT, TDP, TDS>
    {
        public int Id { get; set; }
    }
}
