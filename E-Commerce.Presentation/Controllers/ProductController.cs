using E_Commerce.Presentation.Attributes;
using E_Commerce.Service.Abstraction.Interfaces;
using E_Commerce.Shared.DTOs.ProductDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Presentation.Controllers
{
    public class ProductController : ApiBaseController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        [RedisCache(15)]
        [Authorize]
        public async Task<IActionResult> GetAllProducts([FromQuery] ProductQueryParams productQueryParams)
        {
            var products = await _productService.GetAllProductsAsync(productQueryParams);
            return FromResult(products);
        }
        [HttpGet("Brands")]
        [RedisCache]
        public async Task<IActionResult> GetAllProductBrands()
        {
            var brands = await _productService.GetAllProductBrandsAsync();
            return FromResult(brands);
        }
        [HttpGet("Types")]
        [RedisCache]
        public async Task<IActionResult> GetAllProductTypes()
        {
            var types = await _productService.GetAllProductTypesAsync();
            return FromResult(types);
        }
        [HttpGet("{id:guid}")]
        [RedisCache]
        public async Task<IActionResult> GetProductById([FromRoute] Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            return FromResult(product);
        }
    }
}
