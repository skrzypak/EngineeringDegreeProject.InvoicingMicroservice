using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicingMicroservice.Core.Fluent.Entities
{
    public interface IEntity
    {
        public int Id { get; set; }
        public int CreatedEudId { get; set; }
        public int? LastUpdatedEudId { get; set; }
    }
}
