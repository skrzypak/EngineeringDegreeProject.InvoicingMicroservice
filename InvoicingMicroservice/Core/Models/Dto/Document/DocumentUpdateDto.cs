using InvoicingMicroservice.Core.Fluent.Enums;
using System;

namespace InvoicingMicroservice.Core.Models.Dto.Document
{
    public class DocumentUpdateDto
    {
        public int Id { get; set; }
        public string Signature { get; set; }
        public ushort Number { get; set; }
        public int Type { get; set; }
        public DateTime Date { get; set; }
        public DocumentState State { get; set; }
        public string Description { get; set; }
    }
}
