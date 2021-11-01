using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicingMicroservice.Core.Models.Dto.DocumentType
{
    public class DocumentTypeDto<TD> : DocumentTypeCoreDto
    {
        public int Id { get; set; }
        public virtual ICollection<TD> Documents { get; set; }
    }
}
