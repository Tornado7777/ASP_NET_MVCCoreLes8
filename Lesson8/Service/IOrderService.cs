using Orders.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson8.Service
{
    internal interface IOrderService
    {
        Task<Order> CreatAsync(
            int buyerId, 
            string address, 
            string phone, 
            IEnumerable<(int productId, int quantity)> products);

        public IList<Order> GetAll();

        public Order GetById(int id);
    }
}
