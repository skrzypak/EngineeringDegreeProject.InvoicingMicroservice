using System;
using System.Collections.Generic;
using InvoicingMicroservice.Core.Fluent.Enums;

namespace InvoicingMicroservice.Core.Interfaces.Services
{
    public interface IMicroserviceService
    {
        public object GetSuppliersProductSummary(
            int espId,
            DateTime? startDate,
            DateTime? endDate,
            ICollection<int> suppliersIds,
            ICollection<int> documentTypesIds,
            ICollection<DocumentState> documentStates,
            ICollection<int> productsIds
        );

        public object GetProductsSummary(
            int espId,
            DateTime? startDate,
            DateTime? endDate,
            ICollection<int> documentTypesIds,
            ICollection<DocumentState> documentStates,
            ICollection<int> productsIds
        );
    }
}
