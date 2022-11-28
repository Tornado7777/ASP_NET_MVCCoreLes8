using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Orders.DAL;
using Orders.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Lesson8.Service.Impl
{
    internal class ProductService : IProductService
    {
        private readonly ILogger<ProductService> _logger;
        private readonly OrdersDbContext _context;

        public ProductService(ILogger<ProductService> logger, OrdersDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<Product> CreatAsync(string name, decimal price, string? category)
        {
            var product = new Product
            {
                Name = name,
                Price = price,
                Category = category
            };

            await _context.Products.AddAsync(product);

            await _context.SaveChangesAsync();

            return product;
        }

        public IList<Product> GetAllAsync()
        {
            return _context.Products.ToList();
        }

        public async Task<Product> GetById(int productId)
        {
            var product = await _context.Products.FirstOrDefaultAsync(product => product.Id == productId).ConfigureAwait(false); ;
            if (product == null)
                throw new Exception($"Product with product id = {productId} not found");
            return product;

        }
    }
}
