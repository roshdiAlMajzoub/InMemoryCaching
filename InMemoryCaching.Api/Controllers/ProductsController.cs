using InMemoryCaching.Application.Dtos;
using InMemoryCaching.Domain.Entities;
using InMemoryCaching.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InMemoryCaching.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : Controller
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IProductRepository _productRepository;

        public ProductsController(ILogger<ProductsController> logger, IProductRepository productRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
        }

        [HttpGet(Name = "Products/GetAll")]
        public async Task<IEnumerable<Product>?> GetAll()
        {
            return await _productRepository.GetAllAsync(new CancellationToken());
        }

        [HttpGet("{Id:guid}", Name = "Products/GetById")]
        public async Task<Product?> GetById(Guid Id)
        {
            return await _productRepository.GetByIdAsync(Id,new CancellationToken());
        }
        
        [HttpPost("Products/Add")]
        public IActionResult AddProduct(AddProductDto product)
        {
            _productRepository.Add(product);

            return Ok();
        }


    }
}
