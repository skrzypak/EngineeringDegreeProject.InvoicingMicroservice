using System.Collections.Generic;
using AutoMapper;
using InvoicingMicroservice.Core.Fluent.Entities;
using InvoicingMicroservice.Core.Models.Dto.Document;
using InvoicingMicroservice.Core.Models.Dto.DocumentProduct;
using InvoicingMicroservice.Core.Models.Dto.DocumentType;
using InvoicingMicroservice.Core.Models.Dto.Supplier;
using InvoicingMicroservice.Core.Models.Dto.SupplierContactPerson;

namespace InvoicingMicroservice.Core.Mappers.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap(typeof(SupplierContactPersonCoreDto), typeof(SupplierContactPerson));
            CreateMap(typeof(SupplierCoreDto<SupplierContactPersonCoreDto>), typeof(Supplier));

            CreateMap<DocumentTypeCoreDto, DocumentType>();

            CreateMap<DocumentToProductCoreDto<int>, DocumentToProduct>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Product))
                .ForMember(dest => dest.Product, opt => opt.Ignore())
                .ForMember(dest => dest.DocumentId, opt => opt.Ignore())
                .ForMember(dest => dest.Document, opt => opt.Ignore());

            CreateMap<DocumentCoreDto<int, DocumentToProductCoreDto<int>, int>, Document>()
                .ForMember(dest => dest.DocumentTypeId, opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.DocumentType, opt => opt.Ignore())
                .ForMember(dest => dest.SupplierId, opt => opt.MapFrom(src => src.Supplier))
                .ForMember(dest => dest.Supplier, opt => opt.Ignore())
                .ForMember(dest => dest.DocumentsToProducts, opt => opt.MapFrom(src => src.Products));
        }
    }
}
