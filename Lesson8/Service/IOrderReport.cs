using Lesson8.Models.Reports;
using Orders.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson8.Service
{
    /// <summary>
    /// Интерфейс описывающий механизм генерации чека
    /// </summary>
    public interface IOrderReport
    {
        DateTime OrderDate { get; set; }
        string Address { get; set; }

        string Phone { get; set; }
        string Buyer { get; set; }

        IEnumerable<(int id, string name, string category, decimal price, int quantity, decimal fullPrice )> Products { get; set; }
        public FileInfo Create(string reportFilePath);

        public void CreateReport(IOrderReport reportGenerator, OrderReport order, string reportFileName);
    }
}
