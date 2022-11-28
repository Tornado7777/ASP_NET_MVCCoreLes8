using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.DAL.Entity
{
    [Table("Products")]
    public class Product : NamedEntity
    {
        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        [Unicode(true)]
        public string? Category { get; set; }
    }
}
