using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson8.Models.Reports
{
    public class OrderReport
    {
        public int Id { get; set; } 
        public DateTime OrderDate { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Buyer { get; set; }
        public List<ItemOrder> Products { get; set; }
    }
}
