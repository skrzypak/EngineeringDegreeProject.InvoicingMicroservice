using System.Collections.Generic;
using AutoMapper;
using Comunication.Shared.PayloadValue;
using InvoicingMicroservice.Core.Fluent.Entities;

namespace InvoicingMicroservice.Comunication.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductPayloadValue, Product>();
        }
    }
}
