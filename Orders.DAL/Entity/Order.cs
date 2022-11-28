using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.DAL.Entity
{
    [Table("Orders")]
    public class Order : Entity
    {
        public DateTime OrderDate { get; set; }
        [Required]
        [Unicode(true)]
        public string Address { get; set; } = null!;

        [Required]
        public string Phone { get; set; } = null!;
        [Required]
        [Unicode(true)]
        public Buyer Buyer { get; set; } = null!;

        public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();
    }
}
