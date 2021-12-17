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
        public int SupplierId { get; private set; }
        public int DocumentId { get; private set; }
        public ICollection<ItemsPayloadValue> Items { get; private set; } = new HashSet<ItemsPayloadValue>();
        public class ItemsPayloadValue
        {
            public CRUD Crud { get; set; }
            public int ProductId { get; set; }
            public int DocumentToProductId { get; set; }
            public ushort Quantity { get; set; } = 0;
            public int UnitMeasureValue { get; set; }
            public decimal UnitNetPrice { get; set; }
            public decimal PercentageVat { get; set; }
            public decimal GrossValue { get; set; }
            public DateTime? ExpirationDate { get; set; }
        }
        public int EspId { get; set; }
        public int EudId { get; set; }

        public  class InventoryPayloadValueBuilder 
        {
            private int invoicingSupplierId = -1;
            private int invoicingDocumentId = -1;
            private ICollection<ItemsPayloadValue> items = new HashSet<ItemsPayloadValue>();
            private int espId = -1;
            private int eudId = -1;

            public InventoryPayloadValueBuilder EnterpriseId(int enterpriseId)
            {
                this.espId = enterpriseId;
                return this;
            }

            public InventoryPayloadValueBuilder EnterpriseUserDomainId(int enterpriseUserDomainId)
            {
                this.eudId = enterpriseUserDomainId;
                return this;
            }

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
                if ((model.Transfered == false && crud == CRUD.Create) || (model.Transfered && (crud == CRUD.Update || crud == CRUD.Delete)))
                {
                    this.items.Add(new ItemsPayloadValue()
                    {
                        DocumentToProductId = model.Id,
                        ProductId = model.ProductId,
                        Quantity = model.Quantity,
                        UnitMeasureValue = model.UnitMeasureValue,
                        UnitNetPrice = model.UnitNetPrice,
                        PercentageVat = model.PercentageVat,
                        GrossValue = model.GrossValue,
                        ExpirationDate = model.ExpirationDate,
                        Crud = crud
                    });
                }

                return this;
            }

            public InventoryPayloadValue Build()
            {

                if (this.espId == -1 || this.eudId == -1 || this.invoicingSupplierId == -1 || this.invoicingDocumentId == -1)
                {
                    throw new Exception("Undefined some of obligatory Ids in inventory payload");
                }

                var item = new InventoryPayloadValue
                {
                    SupplierId = this.invoicingSupplierId,
                    DocumentId = this.invoicingDocumentId,
                    Items = this.items,
                    EspId = this.espId,
                    EudId = this.eudId
                };
                return item;
            }
        }

    }

}
