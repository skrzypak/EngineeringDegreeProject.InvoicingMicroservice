using InvoicingMicroservice.Core.Models.Dto.Supplier;
using InvoicingMicroservice.Core.Models.Dto.SupplierContactPerson;

namespace InvoicingMicroservice.Core.Interfaces.Services
{
    public interface ISupplierService
    {
        public object Get(int espId);
        public object GetById(int espId, int supplierId);
        public int Create(int espId, int eudId, SupplierCoreDto<SupplierContactPersonCoreDto> dto);
        public void Delete(int espId, int eudId, int supplierId);
        public object GetContactPersons(int espId, int supplierId);
        public object GetContactPersonById(int espId, int supplierId, int suppContactPersonId);
        public int CreateContactPerson(int espId, int eudId, int suppId, SupplierContactPersonCoreDto dto);
        public void DeleteContactPerson(int espId, int eudId, int supplierId, int suppContactPersonId);
        public void UpdateContactPerson(int espId, int eudId, int supplierId, SupplierContactPersonDto<int> dto);
        public void UpdateSupplier(int espId, int eudId, int supplierId, SupplierDto<SupplierContactPersonDto<int>> dto);
    }
}
