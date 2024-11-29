using CoffeeShopAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_Coffe.Entities;

namespace Project_Coffe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllProducts();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetProductById(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(ProductModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = new Product
            {
                Name = model.Name,
                Price = model.Price,
                Description = model.Description
            };

            await _productService.CreateProduct(product);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProductModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = await _productService.GetProductById(id);
            if (product == null)
                return NotFound();

            product.Name = model.Name;
            product.Price = model.Price;
            product.Description = model.Description;

            await _productService.UpdateProduct(product.Id,product);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.GetProductById(id);
            if (product == null)
                return NotFound();

            await _productService.DeleteProduct(product.Id);
            return NoContent();
        }
    }
}
