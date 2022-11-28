using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplateEngine.Docx;

namespace Lesson8.Service.Impl
{
    internal class ProductReportWord : IProductReport
    {
        #region Private Fields

        private const string _FieldCatalogName = "CatalogName";
        private const string _FieldCatalogDescription = "CatalogDescription";
        private const string _FieldCreateionDate = "CreateionDate";

        private const string _FieldProduct = "Product";

        private const string _FieldProductId = "ProductId";
        private const string _FieldProductName = "ProductName";
        private const string _FieldProductCategory = "ProductCategory";
        private const string _FieldProductPrice = "ProductPrice";

        private const string _FieldProductTotal = "ProductTotal";

        private readonly FileInfo _templateFile;

        #endregion

        #region Public Properties
        public string CatalogName { get; set; } = null!;
        public string CatalogDescription { get; set; } = null!;
        public IEnumerable<(int id, string name, string category, decimal price)> Products { get; set; }
        public DateTime CreationDate { get; set; }

        #endregion


        #region Contrucror
        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateFile">Наименование фала-шаблона</param>
        public ProductReportWord(string templateFile)
        {
            
            _templateFile = new FileInfo(templateFile);
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportFilePath">Наименование фала-отчета</param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public FileInfo Create(string reportFilePath)
        {
            if (!_templateFile.Exists)
                throw new FileNotFoundException();
            var reportFile = new FileInfo(reportFilePath);
            reportFile.Delete();
            _templateFile.CopyTo(reportFile.FullName);

            var rows = Products.Select(p => new TableRowContent( new List<FieldContent>
            {
                new FieldContent(_FieldProductId,p.id.ToString()),
                new FieldContent(_FieldProductName,p.name),
                new FieldContent(_FieldProductCategory,p.category),
                new FieldContent(_FieldProductPrice, p.price.ToString())
            })).ToArray();


            var conent = new Content(
                new FieldContent(_FieldCatalogName, CatalogName),
                new FieldContent(_FieldCatalogDescription, CatalogDescription),
                new FieldContent(_FieldCreateionDate, CreationDate.ToString("dd.MM.yyyy HH:mm:ss")),
                TableContent.Create(_FieldProduct, rows),
                new FieldContent(_FieldProductTotal, Products.Sum(p => p.price).ToString("c"))
                );

            var templateProcessor = new TemplateProcessor(reportFile.FullName)
                .SetRemoveContentControls(true);

            templateProcessor.FillContent(conent);
            templateProcessor.SaveChanges();
            reportFile.Refresh();

            return reportFile;
        }
    }
}
