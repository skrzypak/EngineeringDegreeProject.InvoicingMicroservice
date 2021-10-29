using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicingMicroservice.Core.Models.Dto.Document
{
    public class DocumentDto<TS, TDT, TDP> : DocumentRelationDto<TS, TDT, TDP>
    {
        public int Id { get; set; }
    }
}
