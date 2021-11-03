using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Comunication.Shared.Interfaces;
using InvoicingMicroservice.Core.Fluent.Entities;

namespace Comunication.Shared.PayloadValue
{
    public class InventoryPayloadValue : IMessage
    {
        public static InventoryPayloadValueBuilder Builder => new();
        public int InvoicingSupplierId { get; private set; }
        public int InvoicingDocumentId { get; private set; }
        public ICollection<ItemsPayloadValue> Items { get; private set; } = new HashSet<ItemsPayloadValue>();
        public class ItemsPayloadValue
        {
            public CRUD Crud { get; set; }
            public int ProductId { get; set; }
            public int InvoicingDocumentToProductId { get; set; }
            public ushort NumOfAvailable { get; set; } = 0;
            public DateTime? ExpirationDate { get; set; }
        }

        public  class InventoryPayloadValueBuilder 
        {
            private int invoicingSupplierId;
            private int invoicingDocumentId;
            private ICollection<ItemsPayloadValue> items = new HashSet<ItemsPayloadValue>();

            public InventoryPayloadValueBuilder InvoicingSupplierId(int invoicingSupplierId)
            {
                this.invoicingSupplierId = invoicingSupplierId;
                return this;
            }

            public InventoryPayloadValueBuilder InvoicingDocumentId(int invoicingDocumentId)
            {
                this.invoicingDocumentId = invoicingDocumentId;
                return this;
            }

            public InventoryPayloadValueBuilder AddItems(Document model, CRUD crud)
            {
                this.AddItems(model.DocumentsToProducts, crud);
                return this;
            }

            public InventoryPayloadValueBuilder AddItems(ICollection<DocumentToProduct> models, CRUD crud)
            {
                foreach (var dtp in models)
                {
                    this.AddItem(dtp, crud);
                }
                
                return this;
            }

            public InventoryPayloadValueBuilder AddItem(DocumentToProduct model, CRUD crud)
            {
                this.items.Add(new ItemsPayloadValue()
                {
                    InvoicingDocumentToProductId = model.Id,
                    ProductId = model.ProductId,
                    NumOfAvailable = model.Quantity,
                    ExpirationDate = DateTime.MaxValue,
                    Crud = crud
                });

                return this;
            }

            public InventoryPayloadValue Build()
            {
                var item = new InventoryPayloadValue
                {
                    InvoicingSupplierId = this.invoicingSupplierId,
                    InvoicingDocumentId = this.invoicingDocumentId,
                    Items = this.items
                };
                return item;
            }
        }

    }

}
