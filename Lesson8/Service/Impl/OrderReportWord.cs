using Lesson8.Extensions;
using Lesson8.Models.Reports;
using Orders.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplateEngine.Docx;

namespace Lesson8.Service.Impl
{
    internal class OrderReportWord : IOrderReport
    {
        #region Private Fields

        private const string _FieldAddress = "Address";
        private const string _FieldPhone = "Phone";
        private const string _FieldOrderDate = "OrderDate";
        private const string _FieldBuyer = "Buyer";


        private const string _FieldProduct = "Product";

        private const string _FieldProductId = "ProductId";
        private const string _FieldProductName = "ProductName";
        private const string _FieldProductCategory = "ProductCategory";
        private const string _FieldProductPrice = "ProductPrice";
        private const string _FieldProductQuantity = "ProductQuantity";
        private const string _FieldFullPrice = "FullPrice";

        private const string _FieldProductTotal = "ProductTotal";

        private readonly FileInfo _templateFile;

        #endregion

        #region Public Properties
        public DateTime OrderDate { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Buyer { get; set; }
        public IEnumerable<(
            int id, 
            string name, 
            string category, 
            decimal price, 
            int quantity, 
            decimal fullPrice)> Products { get; set; }
        #endregion

        #region Contrucror
        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateFile">Наименование фала-шаблона</param>
        public OrderReportWord(string templateFile)
        {

            _templateFile = new FileInfo(templateFile);
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportFilePath">Наименование файла-отчета</param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public FileInfo Create(string reportFilePath)
        {
            if (!_templateFile.Exists)
                throw new FileNotFoundException();
            var reportFile = new FileInfo(reportFilePath);
            reportFile.Delete();
            _templateFile.CopyTo(reportFile.FullName);

            var rows = Products.Select(p => new TableRowContent(new List<FieldContent>
            {
                new FieldContent(_FieldProductId,p.id.ToString()),
                new FieldContent(_FieldProductName,p.name),
                new FieldContent(_FieldProductCategory,p.category),
                new FieldContent(_FieldProductPrice, p.price.ToString()),
                new FieldContent(_FieldProductQuantity,p.quantity.ToString()),
                new FieldContent(_FieldFullPrice, p.fullPrice.ToString())
            })).ToArray();


            var conent = new Content(
                new FieldContent(_FieldAddress, Address),
                new FieldContent(_FieldPhone, Phone),
                new FieldContent(_FieldOrderDate, OrderDate.ToString("dd.MM.yyyy HH:mm:ss")),
                new FieldContent(_FieldBuyer, Buyer),
                TableContent.Create(_FieldProduct, rows),
                new FieldContent(_FieldProductTotal, Products.Sum(p => p.fullPrice).ToString("c"))
                );

            var templateProcessor = new TemplateProcessor(reportFile.FullName)
                .SetRemoveContentControls(true);

            templateProcessor.FillContent(conent);
            templateProcessor.SaveChanges();
            reportFile.Refresh();

            return reportFile;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportGenerate">Объект - генератор отчета </param>
        /// <param name="catalog">Объект с данными</param>
        /// <param name="reportFileName">Наименование файла-отчета</param>
        public void CreateReport(IOrderReport reportGenerator, OrderReport order, string reportFileName)
        {
            reportGenerator.Address = order.Address;
            reportGenerator.Phone = order.Phone;
            reportGenerator.OrderDate = order.OrderDate;
            reportGenerator.Buyer = order.Buyer;
            reportGenerator.Products = order.Products.Select(item => (
            item.Id,
            item.Name,
            item.Category,
            item.Price,
            item.Quantity,
            item.FullPrice
            ));
           

            var reportFileInfo = reportGenerator.Create(reportFileName);

            reportFileInfo.Execute();
        }
    }
}
