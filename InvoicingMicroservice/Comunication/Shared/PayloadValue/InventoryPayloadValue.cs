using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Comunication.Shared.Interfaces;

namespace Comunication.Shared.PayloadValue
{
    public class InventoryPayloadValue : IMessage
    {
        public int InvoicingSupplierId { get; set; }
        public int InvoicingDocumentId { get; set; }
        public ICollection<ItemsPayloadValue> Items { get; set; }
        public class ItemsPayloadValue
        {
            public int ProductId { get; set; }
            public int InvoicingDocumentToProductId { get; set; }
            public ushort NumOfAvailable { get; set; }
            public DateTime? ExpirationDate { get; set; }
        }
    }
    
}
