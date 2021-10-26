using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicingMicroservice.Core.Fluent.Entities
{
    public class DeliveryProduct : IEntity
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int DeliveryDocumentId { get; set; }
        public ushort Quantity { get; set; }
        public decimal UnitNetPrice { get; set; }
        public decimal PercentageVat { get; set; }
        public decimal NetValue { get; set; }
        public decimal VatValue { get; set; }
        public decimal GrossValue { get; set; }
        public bool Transfered { get; set; }
        public virtual DeliveryDocument DeliveryDocument { get; set; }
        public virtual Product Product { get; set; }
    }
}
