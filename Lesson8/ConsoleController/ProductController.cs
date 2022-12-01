using Autofac.Core;
using Lesson8.Service;
using Lesson8.Service.Impl;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orders.DAL;
using Orders.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson8.ConsoleController
{
    internal class ProductController
    {
        private static IProductService _productService;
        public static async Task Menu(IProductService productService)
        {
            _productService = productService;
            bool whileOn = true;
            while(whileOn)
            {
                Console.WriteLine("Выберите действие с каталогом продуктов:");
                Console.WriteLine("1. Отобразить все продукты.");
                Console.WriteLine("2. Добавить продукт.");
                Console.WriteLine("3. Выход.\n");
                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.D1:
                        ShowAllProducts();
                        break;
                    case ConsoleKey.D2:
                        await AddProduct();
                        break;
                    case ConsoleKey.D3:
                        whileOn = false;
                        break;
                    default:
                        break;

                }
            }
            
        }

        private static async Task AddProduct()
        {
            Console.Clear();
            Console.WriteLine("Введите наименование продукт: ");
            string name = Console.ReadLine();
            Console.WriteLine("Введите цену продукта: ");
            decimal price = Decimal.Parse(Console.ReadLine());
            Console.WriteLine("Введите категорию продукта: ");
            string category = Console.ReadLine();

            if (name == null)
                throw new Exception("Not input name");
            if (price == 0)
                throw new Exception("Not input price");

            var product = new Product
            {
                Name = name,
                Price = price,
                Category = category
            };

            product = await _productService.CreatAsync(product.Name, product.Price, product.Category);

            Console.WriteLine($"{product.Id} {product.Name} {product.Price} {product.Category} created");

        }

        private static void ShowAllProducts()
        {
            var products = _productService.GetAllAsync();
            Console.WriteLine("{product.Id} {product.Name} {product.Price} {product.Category} ");
            foreach (var product in products)
            {
                Console.WriteLine($"        {product.Id}         {product.Name}     {product.Price}     {product.Category} ");
            }
        }
    }
}
