using System;
using System.Threading.Tasks;
using InvoicingMicroservice.Core.Fluent.Enums;
using InvoicingMicroservice.Core.Models.Dto.Document;
using InvoicingMicroservice.Core.Models.Dto.DocumentProduct;
using InvoicingMicroservice.Core.Models.Dto.DocumentType;

namespace InvoicingMicroservice.Core.Interfaces.Services
{
    public interface IDocumentService
    {
        public object Get(int enterpriseId, int?[] supplierIds, int?[] docTypeIds, DocumentState[] docStates, DateTime? startDate, DateTime? endDate);
        public object GetById(int enterpriseId, int docId);
        public Task<int> Create(int enterpriseId, DocumentCoreDto<int, DocumentToProductCoreDto<int>, int> dto);
        public Task<int> AddProduct(int enterpriseId, int docId, DocumentToProductCoreDto<int> dto);
        public Task TransferProduct(int enterpriseId, int docId, int docProdId);
        public void ChangeDocumentState(int enterpriseId, int docId, DocumentState state);
        public Task Delete(int enterpriseId, int docId, bool hardReset);
        public Task DeleteProduct(int enterpriseId, int docId, int docProdId, bool hardReset);
        public int CreateDocumentType(int enterpriseId, DocumentTypeCoreDto dto);
        public void DeleteDocumentType(int enterpriseId, int docTypeId);
        public object GetDocumentTypeById(int enterpriseId, int docTypeId);
        public object GetDocumentsTypes(int enterpriseId);
    }
}
