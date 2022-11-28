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
    internal class BuyerService : IBuyerService
    {
        private readonly ILogger<BuyerService> _logger;
        private readonly OrdersDbContext _context;

        public BuyerService(ILogger<BuyerService> logger, OrdersDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IList<Buyer> GetAll()
        {
            return _context.Buyers.ToList();
        }

        public async Task<Buyer> GetById(int buyerId)
        {
            var buyer = await _context.Buyers.FirstOrDefaultAsync(buyer => buyer.Id == buyerId).ConfigureAwait(false); ;
            if (buyer == null)
                throw new Exception("Bueyr not found");
            return buyer;
        }
    }
}
