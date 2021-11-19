using System;
using System.Linq;
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

        public int Create(int espId, int eudId, SupplierCoreDto<SupplierContactPersonCoreDto> dto)
        {
            var model = _mapper.Map<SupplierCoreDto<SupplierContactPersonCoreDto>, Supplier>(dto);
            model.EspId = espId;
            model.CreatedEudId = eudId;

            _context.Suppliers.Add(model);
            _context.SaveChanges();

            return model.Id;
        }

        public int CreateContactPerson(int espId, int eudId, int suppId, SupplierContactPersonCoreDto dto)
        {
            var model = _mapper.Map<SupplierContactPersonCoreDto, SupplierContactPerson>(dto);
            model.SupplierId = suppId;
            model.EspId = espId;
            model.CreatedEudId = eudId;

            _context.SuppliersContactsPersons.Add(model);
            _context.SaveChanges();

            return model.Id;
        }

        public void Delete(int espId, int eudId, int supplierId)
        {
            var model = _context.Suppliers
                .FirstOrDefault(s =>
                    s.Id == supplierId &&
                    s.EspId == espId);

            _context.Suppliers.Remove(model);
            _context.SaveChanges();
        }

        public void DeleteContactPerson(int espId, int eudId, int supplierId, int suppContactPersonId)
        {
            var model = _context.SuppliersContactsPersons
                .FirstOrDefault(s =>
                    s.Id == suppContactPersonId &&
                    s.SupplierId == supplierId &&
                    s.EspId == espId);

            _context.SuppliersContactsPersons.Remove(model);
            _context.SaveChanges();
        }

        public object GetContactPersons(int espId, int supplierId)
        {
            var model = _context.SuppliersContactsPersons
                .AsNoTracking()
                .Where(scp => scp.EspId == espId && scp.SupplierId == supplierId)
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

        public object GetContactPersonById(int espId, int supplierId, int suppContactPersonId)
        {
            var model = _context.SuppliersContactsPersons
                .AsNoTracking()
                .Where(scp => scp.EspId == espId && scp.SupplierId == supplierId && scp.Id == suppContactPersonId)
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

        public object GetById(int espId, int supplierId)
        {
            var model = _context.Suppliers
                .AsNoTracking()
                .Where(s => s.EspId == espId && s.Id == supplierId)
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

        public object Get(int espId)
        {
            var model = _context.Suppliers
                .AsNoTracking()
                .Include(s => s.SupplierContactPersons)
                .Where(s => s.EspId == espId)
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
