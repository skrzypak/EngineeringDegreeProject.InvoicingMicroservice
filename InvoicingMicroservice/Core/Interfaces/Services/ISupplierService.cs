using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvoicingMicroservice.Core.Models.Dto.Supplier;
using InvoicingMicroservice.Core.Models.Dto.SupplierContactPerson;

namespace InvoicingMicroservice.Core.Interfaces.Services
{
    public interface ISupplierService
    {
        public object GetSuppliers();
        public object GetSupplierById(int supplierId);
        public int CreateSupplier(SupplierRelationDto<SupplierContactPersonCoreDto> dto);
        public void DeleteSupplier(int supplierId);
        public object GetSuppContactPersons(int supplierId);
        public object GetSuppContactPersonById(int supplierId, int suppContactPersonId);
        public int CreateSuppContactPerson(int suppId, SupplierContactPersonRelationDto<int> dto);
        public void DeleteSuppContactPerson(int supplierId, int suppContactPersonId);

    }
}
