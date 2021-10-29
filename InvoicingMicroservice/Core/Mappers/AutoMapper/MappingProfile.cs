using System.Collections.Generic;
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
            CreateMap(typeof(SupplierContactPersonCoreDto), typeof(SupplierContactPerson));

            CreateMap(typeof(SupplierRelationDto<SupplierContactPersonCoreDto>), typeof(Supplier));
        }
    }
}
