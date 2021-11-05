using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using InvoicingMicroservice.Core.Interfaces.ViewModel;

namespace InvoicingMicroservice.Core.Models.Dto.DocumentProduct
{
    public class DocumentToProductCoreDto<TP> : IDto
    {
        public ushort Quantity { get; set; }
        public ushort UnitMeasureValue { get; set; }
        public decimal UnitNetPrice { get; set; }
        public decimal PercentageVat { get; set; }
        public decimal NetValue { get; set; }
        public decimal VatValue { get; set; }
        public decimal GrossValue { get; set; }
        public bool Transfered { get; set; } = false;
        public DateTime? ExpirationDate { get; set; }
        public virtual TP Product { get; set; }
    }
}
