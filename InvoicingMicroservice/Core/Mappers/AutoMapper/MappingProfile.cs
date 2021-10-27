using AutoMapper;
using InvoicingMicroservice.Core.Fluent.Entities;
using InvoicingMicroservice.Core.Models.Dto.Supplier;
using InvoicingMicroservice.Core.Models.Dto.SupplierContactPerson;

namespace InvoicingMicroservice.Core.Mappers.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SupplierBasicDto, Supplier>();
            CreateMap<Supplier, SupplierBasicDto>();
            CreateMap<SupplierDto, Supplier>();
            CreateMap<Supplier, SupplierDto>();

            CreateMap<SupplierContactPersonBasicDto, SupplierContactPerson>();
            CreateMap<SupplierContactPerson, SupplierContactPersonBasicDto>();
            CreateMap<SupplierContactPersonDto, SupplierContactPerson>();
            CreateMap<SupplierContactPerson, SupplierContactPersonDto>();
        }
    }
}
