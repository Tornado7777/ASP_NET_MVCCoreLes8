using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.DAL.Entity
{
    [Table("Buyers")]
    public class Buyer : NamedEntity
    {
        [Unicode(true)]
        public string? LastName { get; set; }
        [Unicode(true)]
        public string? Patronymic { get; set; }
        public DateTime Birthday { get; set; }
    }
}
