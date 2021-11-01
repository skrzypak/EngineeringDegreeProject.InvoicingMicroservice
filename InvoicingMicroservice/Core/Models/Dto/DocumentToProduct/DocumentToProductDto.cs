using System;

namespace InvoicingMicroservice.Core.Models.Dto.DocumentProduct
{
    public class DocumentToProductDto<TD, TP> : DocumentToProductCoreDto<TP>
    {
        public int Id { get; set; }
        public virtual TD Document { get; set; }
    }
}
