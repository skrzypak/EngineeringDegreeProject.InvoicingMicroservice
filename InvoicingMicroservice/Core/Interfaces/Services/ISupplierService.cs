using InvoicingMicroservice.Core.Models.Dto.Supplier;
using InvoicingMicroservice.Core.Models.Dto.SupplierContactPerson;

namespace InvoicingMicroservice.Core.Interfaces.Services
{
    public interface ISupplierService
    {
        public object Get(int enterpriseId);
        public object GetById(int enterpriseId, int supplierId);
        public int Create(int enterpriseId, SupplierCoreDto<SupplierContactPersonCoreDto> dto);
        public void Delete(int enterpriseId, int supplierId);
        public object GetContactPersons(int enterpriseId, int supplierId);
        public object GetContactPersonById(int enterpriseId, int supplierId, int suppContactPersonId);
        public int CreateContactPerson(int enterpriseId, int suppId, SupplierContactPersonCoreDto dto);
        public void DeleteContactPerson(int enterpriseId, int supplierId, int suppContactPersonId);

    }
}
