using Orders.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson8.Service
{
    internal interface IProductService
    {
        Task<Product> CreatAsync(
           string name,
           decimal price,
           string? category);

        Task<Product> GetById(int productId);

        IList<Product> GetAllAsync();
    }
}
