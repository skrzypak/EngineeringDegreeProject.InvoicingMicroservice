﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicingMicroservice.Core.Models.Dto.Supplier
{
    public class SupplierDto<TSCP> : SupplierRelationDto<TSCP>
    {
        public int Id { get; set; }
    }
}
