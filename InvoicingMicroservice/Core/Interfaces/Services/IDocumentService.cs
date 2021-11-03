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
        public object Get(int?[] supplierIds, int?[] docTypeIds, DocumentState[] docStates, DateTime? startDate, DateTime? endDate);
        public object GetById(int docId);
        public Task<int> Create(DocumentCoreDto<int, DocumentToProductCoreDto<int>, int> dto);
        public Task<int> AddProduct(int docId, DocumentToProductCoreDto<int> dto);
        public Task TransferProduct(int docId, int docProdId);
        public void ChangeDocumentState(int docId, DocumentState state);
        public Task Delete(int docId, bool hardReset);
        public Task DeleteProduct(int docId, int docProdId, bool hardReset);
        public int CreateDocumentType(DocumentTypeCoreDto dto);
        public void DeleteDocumentType(int docTypeId);
        public object GetDocumentTypeById(int docTypeId);
        public object GetDocumentsTypes();
    }
}
