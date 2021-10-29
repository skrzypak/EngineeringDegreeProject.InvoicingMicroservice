using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicingMicroservice.Core.Models.Dto.DocumentProduct
{
    public class DocumentProductRelationDto<TD, TP> : DocumentProductCoreDto
    {
        public virtual TD Document { get; set; }
        public virtual TP Product { get; set; }
    }
}
