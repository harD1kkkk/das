using Microsoft.EntityFrameworkCore;
using Project_Coffe.Entities;
using System;
using Project_Coffe.Data;
using Project_Coffe.Models.ModelInterface;


namespace CoffeeShopAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _dbContext.Products.ToListAsync();
        }

        public async Task<Product> GetProductById(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);
            if (product == null)
            {
                throw new KeyNotFoundException("Product not found.");
            }
            return product;
        }

        public async Task<Product> CreateProduct(Product product)
        {
            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();
            return product;
        }

        public async Task<Product> UpdateProduct(int id, Product product)
        {
            var existingProduct = await _dbContext.Products.FindAsync(id);

            if (existingProduct == null)
            {
                throw new KeyNotFoundException("Product not found.");
            }

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.Stock = product.Stock;

            await _dbContext.SaveChangesAsync();
            return existingProduct;
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);

            if (product == null)
            {
                throw new KeyNotFoundException("Product not found.");
            }

            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}

