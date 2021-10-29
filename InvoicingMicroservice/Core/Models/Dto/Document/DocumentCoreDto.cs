using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using InvoicingMicroservice.Core.Fluent.Enums;
using InvoicingMicroservice.Core.Interfaces.ViewModel;

namespace InvoicingMicroservice.Core.Models.Dto.Document
{
    public class DocumentCoreDto<TDT> : IDto
    {
        public string Signature { get; set; }
        public ushort Number { get; set; }
        public TDT Type { get; set; }
        public DateTime Date { get; set; }
        public DocumentState State { get; set; }
        public string Description { get; set; }
    }
}
