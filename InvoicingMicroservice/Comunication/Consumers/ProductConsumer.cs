using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Comunication.Shared;
using Comunication.Shared.PayloadValue;
using InvoicingMicroservice.Core.Fluent;
using InvoicingMicroservice.Core.Fluent.Entities;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace InvoicingMicroservice.Comunication.Consumers
{
    public class ProductConsumer : IConsumer<Payload<ProductPayloadValue>>
    {
        readonly ILogger<ProductConsumer> _logger;
        private readonly MicroserviceContext _context;

        public ProductConsumer(ILogger<ProductConsumer> logger, MicroserviceContext context)
        {
            _logger = logger;
            _context = context;
        }

        public Task Consume(ConsumeContext<Payload<ProductPayloadValue>> context)
        {
            _logger.LogInformation("Received Product data: {Text}", context.Message.Value);

            switch(context.Message.Type)
            {
                case CRUD.Create:
                {
                    Create(context.Message.Value); 
                    break;
                }
                case CRUD.Update:
                {
                    Update(context.Message.Value);
                    break;
                }
                case CRUD.Delete:
                {
                    Delete(context.Message.Value);
                    break;
                }
            }

            _context.SaveChanges();

            return Task.CompletedTask;
        }

        private void Create(ProductPayloadValue val)
        {
            var model = MapToModel(val);
            _context.Products.Add(model);
        }

        private void Update(ProductPayloadValue val)
        {
            var model = MapToModel(val);
            _context.Products.Update(model);
        }

        private void Delete(ProductPayloadValue val)
        {
            var model = _context.Products.FirstOrDefault(p => p.Id == val.Id);
            _context.Products.Remove(model);
        }

        private Product MapToModel(ProductPayloadValue val)
        {
            return new Product()
            {
                Id = val.Id,
                Code = val.Code,
                Name = val.Name,
                Unit = val.Unit,
                Description = val.Description
            };
        }


    }
}
