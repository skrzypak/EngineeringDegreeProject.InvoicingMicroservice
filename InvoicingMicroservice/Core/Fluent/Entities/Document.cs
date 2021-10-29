using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvoicingMicroservice.Core.Fluent.Enums;

namespace InvoicingMicroservice.Core.Fluent.Entities
{
    public class Document : IEntity
    {
        public int Id { get; set; }
        public int SupplierId { get; set; }
        public int DocumentTypeId { get; set; }
        public string Signature { get; set; }
        public ushort Number { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public DocumentState State { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual DocumentType DocumentType { get; set; }
        public virtual ICollection<DocumentToProduct> DocumentsToProducts { get; set; }
    }
}
