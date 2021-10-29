using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicingMicroservice.Core.Models.Dto.DocumentProduct
{
    public class DocumentProductDto<TD, TP> : DocumentProductRelationDto<TD, TP>
    {
        public int Id { get; set; }
    }
}
