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
        public object GetSuppliersList();
        public object GetSupplierInformations(int supplierId);
        public int CreateSupplier(SupplierCreateDto supplierCreateDto);
        public void DeleteSupplier(int supplierId);
        public object GetSupplierContactPersons(int supplierId);
        public int CreateSupplierContactPerson(int supplierId, SupplierContactPersonBasicDto supplierContactBasicDto);
        public void DeleteSupplierContactPerson(int supplierId, int supplierContactPersonId);

    }
}
