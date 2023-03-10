using System;
using System.Collections.Generic;
using System.Linq;
using InvoicingMicroservice.Core.Exceptions;
using InvoicingMicroservice.Core.Fluent;
using InvoicingMicroservice.Core.Fluent.Enums;
using InvoicingMicroservice.Core.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InvoicingMicroservice.Core.Services
{
    public class MicroserviceService : IMicroserviceService
    {
        private readonly ILogger<DocumentService> _logger;
        private readonly MicroserviceContext _context;

        public MicroserviceService(
            ILogger<DocumentService> logger,
            MicroserviceContext context)
        {
            _logger = logger;
            _context = context;
        }

        public object GetProductsSummary(int espId, DateTime? startDate, DateTime? endDate, ICollection<int> documentTypesIds, ICollection<DocumentState> documentStates, ICollection<int> productsIds)
        {
            object dtos = _context.Products
                .AsNoTracking()
                .Include(p => p.DocumentsToProducts)
                    .ThenInclude(d2p => d2p.Document)
                        .ThenInclude(d => d.DocumentType)
                .Where(p => p.EspId == espId) 
                .Where(p => productsIds.Count > 0 ? productsIds.Contains(p.Id) : true)
                .Select(p => new
                {
                    p.Id,
                    p.Code,
                    p.Name,
                    p.Unit,
                    Values = p.DocumentsToProducts
                    .Where(dtp => startDate == null ? true : dtp.Document.Date.Date >= startDate.Value.Date)
                    .Where(dtp => endDate == null ? true : dtp.Document.Date.Date <= endDate.Value.Date)
                    .Where(dtp => documentTypesIds.Count > 0 ? documentTypesIds.Contains(dtp.Document.DocumentTypeId) : true)
                    .Where(dtp => documentStates.Count > 0 ? documentStates.Contains(dtp.Document.State) : true)
                    .Select(d2p => new
                    {
                        d2p.UnitMeasureValue,
                        d2p.Quantity,
                        d2p.NetValue,
                        d2p.VatValue,
                        d2p.GrossValue
                    })
                })
                .Where(px => px.Values.Count() > 0).ToList()
                .GroupBy(px => new { px.Id, px.Code, px.Name }).Select(pxg => new
                {
                    pxg.Key,
                    Values = pxg.Select(pxgx => new
                    {
                        pxgx.Unit,
                        Values = pxgx.Values.GroupBy(px => new { px.UnitMeasureValue }).Select(x => new
                        {
                            x.Key,
                            TotalQuantity = x.Sum(xg => xg.Quantity),
                            TotalNetValue = x.Sum(xg => xg.NetValue),
                            TotalVatValue = x.Sum(xg => xg.VatValue),
                            TotalGrossValue = x.Sum(xg => xg.GrossValue),
                        }).ToList()
                    })
                }).ToList();


            if (dtos is null)
            {
                throw new NotFoundException($"Empty summary");
            }

            return dtos;
        }

        public object GetSuppliersProductSummary(int espId, DateTime? startDate, DateTime? endDate, ICollection<int> suppliersIds, ICollection<int> documentTypesIds, ICollection<DocumentState> documentStates, ICollection<int> productsIds)
        {

            object dtos = _context.Documents
                .AsNoTracking()
                .Include(doc => doc.Supplier)
                .Include(doc => doc.DocumentType)
                .Include(doc => doc.DocumentsToProducts)
                    .ThenInclude(d2p => d2p.Product)
                .Where(doc => doc.EspId == espId)
                .Where(doc => startDate == null ? true : doc.Date >= startDate)
                .Where(doc => endDate == null ? true : doc.Date <= endDate)
                .Where(doc => suppliersIds.Count > 0 ? suppliersIds.Contains(doc.SupplierId) : true)
                .Where(doc => documentTypesIds.Count > 0 ? documentTypesIds.Contains(doc.DocumentTypeId) : true)
                .Where(doc => documentStates.Count > 0 ? documentStates.Contains(doc.State) : true)
                .Select(doc => new
                {
                    doc.SupplierId,
                    doc.Supplier.CompanyName,
                    doc.Supplier.Nip,
                    doc.Date,
                    doc.State,
                    doc.DocumentType,
                    Products = doc.DocumentsToProducts.Select(d2p => new
                    {
                        d2p.ProductId,
                        d2p.Product.Code,
                        d2p.Product.Name,
                        d2p.Product.Unit,
                        d2p.UnitMeasureValue,
                        d2p.Quantity,
                        d2p.NetValue,
                        d2p.VatValue,
                        d2p.GrossValue,
                    }).Where(d2px => productsIds.Count > 0 ? productsIds.Contains(d2px.ProductId) : true).ToList()
                }).ToList().GroupBy(docx => new { docx.SupplierId, docx.CompanyName, docx.Nip })
                .Select(group => new
                {
                    group.Key,
                    NumberOfDocuments = group.Count(),
                    TotalNetValue = group.Sum(g => g.Products.Sum(p => p.NetValue)),
                    TotalVatValue = group.Sum(g => g.Products.Sum(p => p.VatValue)),
                    TotalGrossValue = group.Sum(g => g.Products.Sum(p => p.GrossValue)),
                    Products = group.SelectMany(x =>
                        x.Products.GroupBy(px => new { px.ProductId, px.Code, px.Name, px.Unit }).Select(d2px => new
                        {
                            d2px.Key,
                            Units = d2px.GroupBy(ux => new { ux.UnitMeasureValue }).Select(uxg => new
                            {
                                uxg.Key,
                                ProductQuantity = uxg.Sum(g => g.Quantity),
                                ProductNetValue = uxg.Sum(g => g.NetValue),
                                ProductVatValue = uxg.Sum(g => g.VatValue),
                                ProductGrossValue = uxg.Sum(g => g.GrossValue),
                            }),
                            totalMeasureUnitValue = d2px.Sum(g => g.UnitMeasureValue),
                            totalNetValue = d2px.Sum(g => g.NetValue),
                            totaltVatValue = d2px.Sum(g => g.VatValue),
                            totalGrossValue = d2px.Sum(g => g.GrossValue)
                        }).ToList()
                    )
                }).ToList();

            //object dtos = _context.Documents
            //    .Include(doc => doc.Supplier)
            //    .Include(doc => doc.DocumentType)
            //    .Include(doc => doc.DocumentsToProducts)
            //        .ThenInclude(d2p => d2p.Product)
            //    .Select(doc => new
            //    {
            //        doc.SupplierId,
            //        doc.Supplier.CompanyName,
            //        doc.Supplier.Nip,
            //        doc.Date,
            //        doc.State,
            //        doc.DocumentType,
            //        Products = doc.DocumentsToProducts.Select(d2p => new
            //        {
            //            d2p.ProductId,
            //            d2p.Product.Code,
            //            d2p.Product.Name,
            //            d2p.Product.Unit,
            //            d2p.UnitMeasureValue,
            //            d2p.Quantity,
            //            d2p.NetValue,
            //            d2p.VatValue,
            //            d2p.GrossValue,
            //        }).ToList()
            //    }).ToList().GroupBy(docx => new { docx.SupplierId, docx.CompanyName, docx.Nip })
            //    .Select(group => new
            //    {
            //        group.Key,
            //        NumberOfDocuments = group.Count(),
            //        TotalNetValue = group.Sum(g => g.Products.Sum(p => p.NetValue)),
            //        TotalVatValue = group.Sum(g => g.Products.Sum(p => p.VatValue)),
            //        TotalGrossValue = group.Sum(g => g.Products.Sum(p => p.GrossValue)),
            //        Products = group.SelectMany(x =>
            //            x.Products.Select(d2p => new
            //            {
            //                d2p.ProductId,
            //                d2p.Code,
            //                d2p.Name,
            //                d2p.Unit,
            //                d2p.UnitMeasureValue,
            //                d2p.Quantity,
            //                d2p.NetValue,
            //                d2p.VatValue,
            //                d2p.GrossValue,
            //            }).ToList().GroupBy(px => new { px.ProductId, px.Code, px.Name }).Select(pxg => new
            //            {
            //                pxg.Key,
            //                Units = pxg.Select(ux => new
            //                {
            //                    ux.Unit,
            //                    ux.UnitMeasureValue,
            //                    ux.Quantity,
            //                    ux.NetValue,
            //                    ux.VatValue,
            //                    ux.GrossValue,
            //                }).ToList().GroupBy(uxg => new { uxg.Unit, uxg.UnitMeasureValue }).Select(uxgg => new
            //                {
            //                    uxgg.Key,
            //                    ProductQuantity = uxgg.Sum(g => g.Quantity),
            //                    ProductNetValue = uxgg.Sum(g => g.NetValue),
            //                    ProductVatValue = uxgg.Sum(g => g.VatValue),
            //                    ProductGrossValue = uxgg.Sum(g => g.GrossValue),
            //                })
            //            }).ToList()
            //        )
            //    }).ToList();

            if (dtos is null)
            {
                throw new NotFoundException($"Empty summary");
            }

            return dtos;
        }
    }
}
