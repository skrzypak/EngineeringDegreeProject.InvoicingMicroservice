using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvoicingMicroservice.Core.Fluent.Enums;

namespace InvoicingMicroservice.Core.Fluent.Entities
{
    public class Product : IEntity
    {
 

        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public UnitType Unit { get; set; }
        public string Description { get; set; }
        public virtual ICollection<DeliveryProduct> DeliveriesProducts { get; set; }
    }
}
