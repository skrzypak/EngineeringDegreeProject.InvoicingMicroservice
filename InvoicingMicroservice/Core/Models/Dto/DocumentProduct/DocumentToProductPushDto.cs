using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using InvoicingMicroservice.Core.Interfaces.ViewModel;

namespace InvoicingMicroservice.Core.Models.Dto.DocumentProduct
{
    public class DocumentToProductPushDto : DocumentToProductCoreDto
    {
        public int ProductId { get; set; }
    }
}
