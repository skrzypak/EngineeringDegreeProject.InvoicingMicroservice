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
        public ICollection<ItemsPayloadValue> Items { get; set; } = new HashSet<ItemsPayloadValue>();
        public class ItemsPayloadValue
        {
            public CRUD Crud { get; set; }
            public int ProductId { get; set; }
            public int InvoicingDocumentToProductId { get; set; }
            public ushort NumOfAvailable { get; set; } = 0;
            public DateTime? ExpirationDate { get; set; }
        }
    }

}
