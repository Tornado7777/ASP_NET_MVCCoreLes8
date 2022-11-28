using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Orders.DAL;
using Orders.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson8.Service.Impl
{
    internal class OrderService : IOrderService
    {
        private readonly ILogger<OrderService> _logger;
        private readonly OrdersDbContext _context;

        public OrderService (ILogger<OrderService> logger, OrdersDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<Order> CreatAsync(int buyerId, string address, string phone, IEnumerable<(int productId, int quantity)> products)
        {
            var buyer = _context.Buyers.FirstOrDefault(buyer => buyer.Id == buyerId); 
            if (buyer == null)
                throw new Exception("Bueyr not found");

            Dictionary<Product, int> productCollection = new Dictionary<Product, int>();
            foreach(var product in products)
            {
                var productEntity = _context.Products.FirstOrDefault(
                    p => p.Id == product.productId);
                if (productEntity == null)
                    throw new Exception("Product not found");
                if (productCollection.ContainsKey(productEntity))
                    productCollection[productEntity]+= product.quantity;
                else
                    productCollection.Add(productEntity, product.quantity);
            }

            var order = new Order
            {
                Buyer = buyer,
                Address = address,
                Phone = phone,
                OrderDate = DateTime.Now,
                Items = productCollection.Select(p => new OrderItem
                {
                    Product = p.Key,
                    Quantity = p.Value
                }).ToArray()
            };

            await _context.Orders.AddAsync(order);

            _context.SaveChanges();

            return order;

        }

        public IList<Order> GetAll()
        {
            return _context.Orders.Include(order => order.Buyer).Include(order => order.Items).ThenInclude(item => item.Product).ToList();
        }

        public Order GetById(int id)
        {
            return _context.Orders.Include(order => order.Buyer).Include(order => order.Items).ThenInclude(item => item.Product).FirstOrDefault(order => order.Id == id);
        }
    }
}
