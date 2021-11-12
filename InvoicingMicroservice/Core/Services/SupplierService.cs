using System;
using System.Linq;
using Authentication;
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
        private readonly IHeaderContextService _headerContextService;

        public SupplierService(ILogger<SupplierService> logger,
            MicroserviceContext context,
            IMapper mapper,
            IHeaderContextService headerContextService)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
            _headerContextService = headerContextService;
        }

        public int Create(int enterpriseId, SupplierCoreDto<SupplierContactPersonCoreDto> dto)
        {
            var model = _mapper.Map<SupplierCoreDto<SupplierContactPersonCoreDto>, Supplier>(dto);
            model.EspId = enterpriseId;
            model.CreatedEudId = _headerContextService.GetEnterpriseUserDomainId(enterpriseId);

            _context.Suppliers.Add(model);
            _context.Entry(model).Property("CreatedDate").CurrentValue = DateTime.Now;
            _context.SaveChanges();

            return model.Id;
        }

        public int CreateContactPerson(int enterpriseId, int suppId, SupplierContactPersonCoreDto dto)
        {
            var model = _mapper.Map<SupplierContactPersonCoreDto, SupplierContactPerson>(dto);
            model.SupplierId = suppId;
            model.EspId = enterpriseId;
            model.CreatedEudId = _headerContextService.GetEnterpriseUserDomainId(enterpriseId);

            _context.SuppliersContactsPersons.Add(model);
            _context.Entry(model).Property("CreatedDate").CurrentValue = DateTime.Now;
            _context.SaveChanges();

            return model.Id;
        }

        public void Delete(int enterpriseId, int supplierId)
        {
            var model = new Supplier() { Id = supplierId, EspId = enterpriseId };

            _context.Suppliers.Attach(model);
            _context.Suppliers.Remove(model);
            _context.SaveChanges();
        }

        public void DeleteContactPerson(int enterpriseId, int supplierId, int suppContactPersonId)
        {
            var model = new SupplierContactPerson() { Id = suppContactPersonId, SupplierId = supplierId, EspId = enterpriseId };

            _context.SuppliersContactsPersons.Attach(model);
            _context.SuppliersContactsPersons.Remove(model);
            _context.SaveChanges();
        }

        public object GetContactPersons(int enterpriseId, int supplierId)
        {
            var model = _context.SuppliersContactsPersons
                .AsNoTracking()
                .Where(scp => scp.EspId == enterpriseId && scp.SupplierId == supplierId)
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

        public object GetContactPersonById(int enterpriseId, int supplierId, int suppContactPersonId)
        {
            var model = _context.SuppliersContactsPersons
                .AsNoTracking()
                .Where(scp => scp.EspId == enterpriseId && scp.SupplierId == supplierId && scp.Id == suppContactPersonId)
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

        public object GetById(int enterpriseId, int supplierId)
        {
            var model = _context.Suppliers
                .AsNoTracking()
                .Where(s => s.EspId == enterpriseId && s.Id == supplierId)
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

        public object Get(int enterpriseId)
        {
            var model = _context.Suppliers
                .AsNoTracking()
                .Include(s => s.SupplierContactPersons)
                .Where(s => s.EspId == enterpriseId)
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
