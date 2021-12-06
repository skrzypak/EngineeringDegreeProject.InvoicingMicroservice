using System;
using System.Collections.Generic;
using System.Linq;
using InvoicingMicroservice.Core.Exceptions;
using InvoicingMicroservice.Core.Fluent;
using InvoicingMicroservice.Core.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InvoicingMicroservice.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly ILogger<ProductService> _logger;
        private readonly MicroserviceContext _context;

        public ProductService(ILogger<ProductService> logger, MicroserviceContext context)
        {
            _logger = logger;
            _context = context;
        }

        public object Get(int espId)
        {

            var dtos = _context
               .Products
               .AsNoTracking()
               .Where(p => p.EspId == espId)
               .Select(p => new {
                   p.Id,
                   p.Code,
                   p.Name,
                   p.Unit,
                   p.Description,
               })
               .OrderBy(px => px.Name)
               .ToHashSet();

            if (dtos is null)
            {
                throw new NotFoundException($"NOT FOUND any product");
            }

            return dtos;
        }

    }
}
