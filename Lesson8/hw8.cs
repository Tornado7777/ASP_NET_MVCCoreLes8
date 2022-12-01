using Autofac;
using Autofac.Configuration;
using Autofac.Extensions.DependencyInjection;
using Lesson8.Autofac;
using Lesson8.ConsoleController;
using Lesson8.Models.Reports;
using Lesson8.Service;
using Lesson8.Service.Impl;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orders.DAL;
using Orders.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Lesson8.Extensions;

namespace Lesson8
{
    internal class hw8
    {
        private static WebApplication? _app;

        public static WebApplication App 
        {
            get 
            {
                if (_app == null)
                {
                    _app = CreateHostBuilder(Environment.GetCommandLineArgs()).Build();

                    if (_app.Environment.IsDevelopment())
                    {
                        _app.UseDeveloperExceptionPage();
                        //_app.UseExceptionHandler("/Error");
                        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                        _app.UseHsts();
                    }

                    _app.UseStaticFiles();

                    _app.UseRouting();

                    _app.UseAuthorization();

                    _app.MapControllerRoute(
                        name: "defualt",
                        pattern: "{controller=Home}/{action=Index}/{id?}"
                        );


                }
                return _app;
            }
        }
            //=> _host ??= CreateHostBuilder(Environment.GetCommandLineArgs()).Build();

        private static IServiceProvider Services => App.Services;
        static async Task Main(string[] args)
        {
            var app = App;
            await app.StartAsync();
            //await PrintBuyersAsync();
            //await ProductMenu();
            //await OrderMenu();
            Console.ReadKey();
            await app.StopAsync();
        }

        private static async Task ProductMenu()
        {
            await using var serviceScope = Services.CreateAsyncScope();
            var services = serviceScope.ServiceProvider;

            var context = services.GetRequiredService<OrdersDbContext>();
            var logger = services.GetRequiredService<ILogger<ProductController>>();
            var productService = services.GetRequiredService<ProductService>();

            await ProductController.Menu(productService);

        }

        private static async Task OrderMenu()
        {
            await using var serviceScope = Services.CreateAsyncScope();
            var services = serviceScope.ServiceProvider;

            var context = services.GetRequiredService<OrdersDbContext>();
            var logger = services.GetRequiredService<ILogger<ProductController>>();
            var productService = services.GetRequiredService<ProductService>();
            var orderService = services.GetRequiredService<OrderService>();
            var buyerService = services.GetRequiredService<BuyerService>();

            await OrderController.Menu(productService, orderService, buyerService);

        }

        private static async Task PrintBuyersAsync()
        {
            var rnd = new Random();

            await using var serviceScope = Services.CreateAsyncScope();
            var services = serviceScope.ServiceProvider;

            var context = services.GetRequiredService<OrdersDbContext>();
            var logger = services.GetRequiredService<ILogger<hw8>>();

            foreach (var buyer in context.Buyers)
            {
                logger.LogInformation($"Покупапатель >>> {buyer.LastName} {buyer.Name} {buyer.Patronymic} {buyer.Birthday.ToShortDateString()}");
            }
            var orderService = services.GetRequiredService<IOrderService>();

            Console.WriteLine("PrintBuyersAsync\n");
            //await orderService.CreatAsync(
            //    rnd.Next(1, 5),
            //    "123,Russian, Address",
            //    "+7(903)-000-00-01",
            //    new (int, int)[]
            //    {
            //        new ValueTuple<int,int>(1, 1)
            //    });

            var productCatalog = new ProductsCatalog
            {
                Name = "Каталог товаров",
                Description = "Актуальный список товаров на дату",
                CreationDate = DateTime.Now,
                Products = context.Products
            };

            string templateFile = "Templates/DefualtTemplate.docx";
            IProductReport report = new ProductReportWord(templateFile);
            CreateReport(report, productCatalog, "ReportProducts.docx");

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportGenerate">Объект - генератор отчета </param>
        /// <param name="catalog">Объект с данными</param>
        /// <param name="reportFileName">Наименование файла-отчета</param>
        private static void CreateReport(IProductReport reportGenerator, ProductsCatalog catalog, string reportFileName )
        {
            reportGenerator.CatalogName = catalog.Name;
            reportGenerator.CatalogDescription = catalog.Description;
            reportGenerator.CreationDate = catalog.CreationDate;
            reportGenerator.Products = catalog.Products.Select(p => (p.Id, p.Name, p.Category, p.Price));

            var reportFileInfo = reportGenerator.Create(reportFileName);

            reportFileInfo.Execute();
        }

        public static WebApplicationBuilder CreateHostBuilder(string[] args)
        {
            var webApplicationBuilder = WebApplication.CreateBuilder(args);


            webApplicationBuilder.Host
                 .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                 .ConfigureContainer<ContainerBuilder>(container => //autofac
                 {

                     container.RegisterType<OrderService>().InstancePerLifetimeScope();
                     container.RegisterType<ProductService>().InstancePerLifetimeScope();
                     container.RegisterType<BuyerService>().InstancePerLifetimeScope();


                 })
                 .ConfigureHostConfiguration(options =>
                 options.AddJsonFile("appsetting.json"))
                 .ConfigureAppConfiguration(options =>
                 options
                     .AddJsonFile("appsetting.json")

                     .AddEnvironmentVariables()
                     .AddCommandLine(args))
                 .ConfigureLogging(options =>
                 options
                     .ClearProviders() //using Microsoft.Extensions.Logging;
                     .AddConsole()
                     .AddDebug())
                 .ConfigureServices(ConfigureServices);

            return webApplicationBuilder;
        }

        private static void ConfigureServices(HostBuilderContext host, IServiceCollection services)
        {

            services.AddControllersWithViews();

            #region Register Base sServices

            //services.AddTransient<IOrderService, OrderService>();

            #endregion


            services.AddDbContext<OrdersDbContext>(options =>
            {
                options
                .UseSqlServer(host.Configuration["Settings:DatabseOptions:ConnectionsString"]);
            });

        }
    }
}
