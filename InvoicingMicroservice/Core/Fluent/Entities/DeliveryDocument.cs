using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicingMicroservice.Core.Fluent.Entities
{
    public class DeliveryDocument : IEntity
    {
        public int Id { get; set; }
        public int SupplierId { get; set; }
        public string Signature { get; set; }
        public ushort Number { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<DeliveryProduct> DeliveriesProducts { get; set; }
    }
}
