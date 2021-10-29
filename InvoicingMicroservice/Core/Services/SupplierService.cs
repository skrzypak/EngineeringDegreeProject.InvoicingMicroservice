using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using InvoicingMicroservice.Core.Exceptions;
using InvoicingMicroservice.Core.Fluent;
using InvoicingMicroservice.Core.Fluent.Entities;
using InvoicingMicroservice.Core.Interfaces.Services;
using InvoicingMicroservice.Core.Models.Dto.Supplier;
using InvoicingMicroservice.Core.Models.Dto.SupplierContactPerson;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InvoicingMicroservice.Core.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ILogger<SupplierService> _logger;
        private readonly MicroserviceContext _context;
        private readonly IMapper _mapper;

        public SupplierService(ILogger<SupplierService> logger,
            MicroserviceContext context,
            IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        public int CreateSupplier(SupplierRelationDto<SupplierContactPersonCoreDto> dto)
        {
            var model = _mapper.Map<SupplierRelationDto<SupplierContactPersonCoreDto>, Supplier>(dto);

            _context.Suppliers.Add(model);
            _context.SaveChanges();

            return model.Id;
        }

        public int CreateSuppContactPerson(int suppId, SupplierContactPersonRelationDto<int> dto)
        {
            var model = _mapper.Map<SupplierContactPersonRelationDto<int>, SupplierContactPerson>(dto);
            model.SupplierId = suppId;

            _context.SuppliersContactsPersons.Add(model);
            _context.SaveChanges();

            return model.Id;
        }

        public void DeleteSupplier(int supplierId)
        {
            var model = new Supplier() { Id = supplierId };

            _context.Suppliers.Attach(model);
            _context.Suppliers.Remove(model);
            _context.SaveChanges();
        }

        public void DeleteSuppContactPerson(int supplierId, int suppContactPersonId)
        {
            var model = new SupplierContactPerson() { Id = suppContactPersonId, SupplierId = supplierId };

            _context.SuppliersContactsPersons.Attach(model);
            _context.SuppliersContactsPersons.Remove(model);
            _context.SaveChanges();
        }

        public object GetSuppContactPersons(int supplierId)
        {
            var model = _context.SuppliersContactsPersons
                .AsNoTracking()
                .Where(scp => scp.SupplierId == supplierId)
                .Select(scp => new {
                        scp.Id,
                        scp.FirstName,
                        scp.LastName,
                        scp.Email,
                        scp.PhoneNumber,
                        scp.Description
                    })
                .AsEnumerable()
                .OrderBy(scpx => scpx.LastName).ThenBy(scpx => scpx.FirstName);

            if (model is null)
            {
                throw new NotFoundException($"NOT FOUND any persons");
            }

            return model;
        }

        public object GetSuppContactPersonById(int supplierId, int suppContactPersonId)
        {
            var model = _context.SuppliersContactsPersons
                .AsNoTracking()
                .Where(scp => scp.SupplierId == supplierId && scp.Id == suppContactPersonId)
                .Select(scp => new
                {
                    scp.Id,
                    scp.FirstName,
                    scp.LastName,
                    scp.Email,
                    scp.PhoneNumber,
                    scp.Description
                });

            if (model is null)
            {
                throw new NotFoundException($"Contact person NOT FOUND");
            }

            return model;
        }

        public object GetSupplierById(int supplierId)
        {
            var model = _context.Suppliers
                .AsNoTracking()
                .Where(s => s.Id == supplierId)
                .Include(s => s.SupplierContactPersons)
                .Select(s => new
                {
                    s.Id,
                    s.Address,
                    s.Archive,
                    s.City,
                    s.Code,
                    s.CompanyName,
                    s.Description,
                    s.Email,
                    s.Fax,
                    s.Homepage,
                    s.Krs,
                    s.Nip,
                    s.PhoneNumber,
                    s.PostalCode,
                    s.Regon,
                    s.State,
                    s.StreetAddress,
                    SuppliersContactsPersons = s.SupplierContactPersons.Select(scp => new
                        {
                            scp.Id,
                            scp.FirstName,
                            scp.LastName,
                            scp.Email,
                            scp.PhoneNumber,
                            scp.Description
                        })
                        .OrderBy(sx => sx.LastName).ThenBy(sx => sx.FirstName)
                        .Take(5)
                        .ToList(),
                    Documents = s.Documents.Select(dd => new
                        {
                            dd.Id,
                            dd.Signature,
                            dd.Number,
                            dd.Description,
                            dd.Date
                        })
                        .OrderByDescending(sx => sx.Date)
                        .Take(5)
                        .ToList()
                })
                .FirstOrDefault();

            if (model is null)
            {
                throw new NotFoundException($"Supplier with id {supplierId} NOT FOUND");
            }

            return model;
        }

        public object GetSuppliers()
        {
            var model = _context.Suppliers
                .AsNoTracking()
                .Include(s => s.SupplierContactPersons)
                .Select(s => new
                {
                    s.Id,
                    s.Code,
                    s.CompanyName,
                    s.Description,
                    s.Email,
                    s.Nip,
                    s.PhoneNumber,
                    s.City,
                    s.PostalCode,
                    s.State,
                    s.StreetAddress,
                })
                .ToList();

            if (model is null)
            {
                throw new NotFoundException($"NOT FOUND any supplier");
            }

            return model;
        }
    }
}
