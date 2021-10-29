using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using InvoicingMicroservice.Core.Interfaces.ViewModel;

namespace InvoicingMicroservice.Core.Models.Dto.DocumentType
{
    public class DocumentTypeCoreDto : IDto
    {
        [MaxLength(6)]
        public string Code { get; set; }
        [MinLength(1), MaxLength(300)]
        public string Name { get; set; }
        [MaxLength(3000)]
        public string Description { get; set; }
    }
}
