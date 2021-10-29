using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicingMicroservice.Core.Models.Dto.DocumentType
{
    public class DocumentTypeRelationDto<TD> : DocumentTypeCoreDto
    {
        public virtual ICollection<TD> Documents { get; set; }
    }
}
