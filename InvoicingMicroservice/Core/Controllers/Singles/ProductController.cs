using Authentication;
using InvoicingMicroservice.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InvoicingMicroservice.Core.Controllers.Single
{
    [ApiController]
    [Route("/api/invoicing/1.0.0/products")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;
        private readonly IHeaderContextService _headerContextService;

        public ProductController(ILogger<ProductController> logger, IProductService productService, IHeaderContextService headerContextService)
        {
            _logger = logger;
            _productService = productService;
            _headerContextService = headerContextService;
        }

        
        [HttpGet]
        public ActionResult<object> GetDishAllergens([FromQuery] int espId)
        {
            var response = _productService.Get(espId);
            return Ok(response);
        }

    }
}
