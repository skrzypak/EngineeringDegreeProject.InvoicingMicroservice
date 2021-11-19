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
        public object Get(int espId, int?[] supplierIds, int?[] docTypeIds, DocumentState[] docStates, DateTime? startDate, DateTime? endDate);
        public object GetById(int espId, int docId);
        public Task<int> Create(int espId, int eudId, DocumentCoreDto<int, DocumentToProductCoreDto<int>, int> dto);
        public Task<int> AddProduct(int espId, int eudId, int docId, DocumentToProductCoreDto<int> dto);
        public Task TransferProduct(int espId, int eudId, int docId, int docProdId);
        public void ChangeDocumentState(int espId, int eudId, int docId, DocumentState state);
        public Task Delete(int espId, int eudId, int docId, bool hardReset);
        public Task DeleteProduct(int espId, int eudId, int docId, int docProdId, bool hardReset);
        public int CreateDocumentType(int espId, int eudId, DocumentTypeCoreDto dto);
        public void DeleteDocumentType(int espId, int eudId, int docTypeId);
        public object GetDocumentTypeById(int espId, int docTypeId);
        public object GetDocumentsTypes(int espId);
    }
}
