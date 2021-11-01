using InvoicingMicroservice.Core.Models.Dto.Supplier;
using InvoicingMicroservice.Core.Models.Dto.SupplierContactPerson;

namespace InvoicingMicroservice.Core.Interfaces.Services
{
    public interface ISupplierService
    {
        public object Get();
        public object GetById(int supplierId);
        public int Create(SupplierCoreDto<SupplierContactPersonCoreDto> dto);
        public void Delete(int supplierId);
        public object GetContactPersons(int supplierId);
        public object GetContactPersonById(int supplierId, int suppContactPersonId);
        public int CreateContactPerson(int suppId, SupplierContactPersonCoreDto dto);
        public void DeleteContactPerson(int supplierId, int suppContactPersonId);

    }
}
