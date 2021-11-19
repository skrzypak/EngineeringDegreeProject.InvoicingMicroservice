using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Comunication;
using Comunication.Shared;
using Comunication.Shared.PayloadValue;
using InvoicingMicroservice.Core.Exceptions;
using InvoicingMicroservice.Core.Fluent;
using InvoicingMicroservice.Core.Fluent.Entities;
using InvoicingMicroservice.Core.Fluent.Enums;
using InvoicingMicroservice.Core.Interfaces.Services;
using InvoicingMicroservice.Core.Models.Dto.Document;
using InvoicingMicroservice.Core.Models.Dto.DocumentProduct;
using InvoicingMicroservice.Core.Models.Dto.DocumentType;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InvoicingMicroservice.Core.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly ILogger<DocumentService> _logger;
        private readonly MicroserviceContext _context;
        private readonly IMapper _mapper;
        private readonly IBus _bus;
        private readonly RabbitMq _rabbitMq;

        public DocumentService(
            ILogger<DocumentService> logger,
            MicroserviceContext context,
            IMapper mapper,
            IBus bus,
            RabbitMq rabbitMq)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
            _bus = bus;
            _rabbitMq = rabbitMq;
        }

        public void ChangeDocumentState(int espId, int eudId, int docId, DocumentState state)
        {
            var model = _context.Documents
                .FirstOrDefault(d => d.EspId == espId && d.Id == docId);

            if(model is null)
            {
                throw new NotFoundException($"Document with id {docId} NOT FOUND");
            }

            model.LastUpdatedEudId = eudId;

            model.State = state;
            _context.Entry(model).Property("LastUpdatedDate").CurrentValue = DateTime.Now;
            _context.SaveChanges();
        }

        public int CreateDocumentType(int espId, int eudId, DocumentTypeCoreDto dto)
        {
            var model = _mapper.Map<DocumentTypeCoreDto, DocumentType>(dto);
            model.EspId = espId;
            model.CreatedEudId = eudId;

            _context.DocumentsTypes.Add(model);
            _context.SaveChanges();

            return model.Id;
        }

        public async Task<int> AddProduct(int espId, int eudId, int docId, DocumentToProductCoreDto<int> dto)
        {
            var model = _mapper.Map<DocumentToProductCoreDto<int>, DocumentToProduct>(dto);
            model.DocumentId = docId;
            model.EspId = espId;
            model.CreatedEudId = eudId;

            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.DocumentToProducts.Add(model);
                        await _context.SaveChangesAsync();

                        model = _context.DocumentToProducts
                            .Include(dtp => dtp.Document)
                            .First(dtp => dtp.EspId == espId && dtp.Id == model.Id);

                        if (dto.Transfered == false)
                        {
                            var message = InventoryPayloadValue.Builder
                                .InvoicingSupplierId(model.Document.SupplierId)
                                .InvoicingDocumentId(model.DocumentId)
                                .EnterpriseId(espId)
                                .EnterpriseUserDomainId(eudId)
                                .AddItem(model, CRUD.Create)
                                .Build();

                            await SyncAsync(message, CRUD.Update);

                            model.Transfered = true;
                            await _context.SaveChangesAsync();
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception(ex.Message);
                    }
                }
            });

            return model.Id;
        }

        public async Task<int> Create(int espId, int eudId, DocumentCoreDto<int, DocumentToProductCoreDto<int>, int> dto)
        {
            var model = _mapper.Map<DocumentCoreDto<int, DocumentToProductCoreDto<int>, int>, Document>(dto);
            model.EspId = espId;
            model.CreatedEudId = eudId;

            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {

                        _context.Documents.Add(model);
                        await _context.SaveChangesAsync();

                        var message = InventoryPayloadValue.Builder
                            .InvoicingSupplierId(model.SupplierId)
                            .InvoicingDocumentId(model.Id)
                            .EnterpriseId(espId)
                            .EnterpriseUserDomainId(eudId)
                            .AddItems(model.DocumentsToProducts, CRUD.Create)
                            .Build();

                        if (message.Items.Count > 0)
                        {
                            await SyncAsync(message, CRUD.Create);
                        }

                        foreach (var m in model.DocumentsToProducts)
                        {
                            m.Transfered = true;
                        }
                        _context.Documents.Update(model);
                        await _context.SaveChangesAsync();

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception(ex.Message);
                    }
                }
            });
                
            return model.Id;
        }

        public async Task Delete(int espId, int eudId, int docId, bool hardReset)
        {
            var model = _context.Documents
                .FirstOrDefault(d => d.EspId == espId && d.Id == docId);

            if (model is null)
            {
                throw new NotFoundException($"Document with id {docId} NOT FOUND");
            }

            _context.Documents.Remove(model);

            if (hardReset)
            {
                var message = InventoryPayloadValue.Builder
                    .InvoicingSupplierId(model.SupplierId)
                    .InvoicingDocumentId(model.Id)
                    .EnterpriseId(espId)
                    .EnterpriseUserDomainId(eudId)
                    .Build();

                await SyncAsync(message, CRUD.Delete);
            }

            _context.SaveChanges();
        }

        public async Task DeleteProduct(int espId, int eudId, int docId, int docProdId, bool hardReset)
        {
            var model = _context.DocumentToProducts
                .Include(dtp => dtp.Document)
                .FirstOrDefault(dtp => dtp.EspId == espId && dtp.Id == docProdId && dtp.DocumentId == docId);

            if (model is null)
            {
                throw new NotFoundException($"Document Product with id {docProdId} NOT FOUND");
            }

            _context.DocumentToProducts.Remove(model);

            if (hardReset && model.Transfered == true)
            {
                var message = InventoryPayloadValue.Builder
                    .InvoicingSupplierId(model.Document.SupplierId)
                    .InvoicingDocumentId(model.DocumentId)
                    .AddItem(model, CRUD.Delete)
                    .Build();

                await SyncAsync(message, CRUD.Update);
            }

            _context.SaveChanges();
        }

        public async Task TransferProduct(int espId, int eudId, int docId, int docProdId)
        {
            var model = _context.DocumentToProducts
                .AsNoTracking()
                .Include(dtp => dtp.Document)
                .FirstOrDefault(dtp => dtp.EspId == espId && dtp.Id == docProdId && dtp.DocumentId == docId);

            model.LastUpdatedEudId = eudId;

            if (model is null)
            {
                throw new NotFoundException($"Document Product with id {docProdId} NOT FOUND");
            }

            if (model.Transfered == true)
            {
                throw new TransferException($"Document Product with id {docProdId} ALLREADY TRANSFERED");
            }

            var message = InventoryPayloadValue.Builder
                .InvoicingSupplierId(model.Document.SupplierId)
                .InvoicingDocumentId(model.DocumentId)
                .EnterpriseId(espId)
                .EnterpriseUserDomainId(eudId)
                .AddItem(model, CRUD.Create)
                .Build();

            await SyncAsync(message, CRUD.Update);

            model.Transfered = true;
            _context.DocumentToProducts.Update(model);
            _context.Entry(model).Property("LastUpdatedDate").CurrentValue = DateTime.Now;
            _context.SaveChanges();
        }

        public void DeleteDocumentType(int espId, int eudId, int docTypeId)
        {
            var model = _context.DocumentsTypes
                .FirstOrDefault(dt =>
                    dt.Id == docTypeId &&
                    dt.EspId == espId);

            _context.DocumentsTypes.Remove(model);
            _context.SaveChanges();
        }

        public object GetById(int espId, int docId)
        {
            var dto = _context.Documents
                .AsNoTracking()
                .Include(d => d.Supplier)
                .Include(d => d.DocumentType)
                .Include(d => d.DocumentsToProducts)
                    .ThenInclude(dtp => dtp.Product)
                .Where(d => d.EspId == espId &&  d.Id == docId)
                .Select(d => new
                {
                    d.Id,
                    d.SupplierId,
                    d.Signature,
                    d.Number,
                    d.Description,
                    d.Date,
                    d.DocumentType,
                    DocumentProducts = d.DocumentsToProducts.Select(dtp => new
                    {
                        dtp.Id,
                        dtp.Quantity,
                        dtp.UnitMeasureValue,
                        dtp.UnitNetPrice,
                        dtp.PercentageVat,
                        dtp.NetValue,
                        dtp.VatValue,
                        dtp.GrossValue,
                        dtp.Transfered,
                        Product = new
                        {
                            dtp.Product.Id,
                            dtp.Product.Code,
                            dtp.Product.Name,
                            dtp.Product.Unit,
                            dtp.Product.Description
                        }
                    })
                })
                .FirstOrDefault();

            if(dto is null)
            {
                throw new NotFoundException($"Document with id {dto.Id} NOT FOUND");
            }

            return dto;
        }

        public object Get(int espId, int?[] supplierId, int?[] docTypeIds, DocumentState[] docStates, DateTime? startDate, DateTime? endDate)
        {
            var iquery = _context.Documents
               .AsNoTracking()
               .Include(d => d.Supplier)
               .Include(d => d.DocumentType)
               .Where(d => d.EspId == espId);

            IQueryable<Document> query = iquery;

            if (supplierId is not null && supplierId.Length > 0)
            {
                query = query.Where(d => supplierId.Contains(d.SupplierId));
            }

            if (docTypeIds is not null && docTypeIds.Length > 0)
            {
                query = query.Where(d => docTypeIds.Contains(d.DocumentTypeId));
            }

            if (docStates is not null && docStates.Length > 0)
            {
                query = query.Where(d => docStates.Contains(d.State));
            }

            if (startDate is not null)
            {
                query = query.Where(d => d.Date >= startDate);
            }

            if (endDate is not null)
            {
                query = query.Where(d => d.Date <= endDate);
            }

            var dtos = query.Select(d => new
                {
                    d.Id,
                    d.SupplierId,
                    d.Signature,
                    d.Number,
                    d.DocumentType,
                    d.State,
                    d.Date,
                    d.Description,
                })
            .ToList()
            .OrderByDescending(dx => dx.Date);

            if (dtos is null)
            {
                throw new NotFoundException("Not found any documents");
            }

            return dtos;
        }

        public object GetDocumentsTypes(int espId)
        {
            var dto = _context.DocumentsTypes
                .AsNoTracking()
                .Where(dt => dt.EspId == espId)
                .Select(d => new
                {
                    d.Id,
                    d.Code,
                    d.Name,
                    d.Description
                })
                .AsEnumerable();

            if (dto is null)
            {
                throw new NotFoundException($"NOT FOUND any document types");
            }

            return dto;
        }

        public object GetDocumentTypeById(int espId, int docTypeId)
        {
            var dto = _context.DocumentsTypes
                .AsNoTracking()
                .Where(d => d.EspId == espId && d.Id == docTypeId)
                .Select(d => new
                {
                    d.Id,
                    d.Code,
                    d.Name,
                    d.Description
                })
                .FirstOrDefault();

            if (dto is null)
            {
                throw new NotFoundException($"Document type with id {dto.Id} NOT FOUND");
            }

            return dto;
        }

        private async Task SyncAsync(InventoryPayloadValue message, CRUD crud)
        {
            var payload = new Payload<InventoryPayloadValue>(message, crud);

            Uri[] uri = {
                new Uri($"{_rabbitMq.Host}/msinve.inventory.queue"),
            };

            CancellationTokenSource s_cts = new CancellationTokenSource();
            s_cts.CancelAfter(5000);

            try
            {

                foreach (var u in uri)
                {
                    var endPoint = await _bus.GetSendEndpoint(u);
                    await endPoint.Send(payload, s_cts.Token);
                }

            } 
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                s_cts.Dispose();
            }
        }

    }
}
