using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVC_testovoe.Models
{
    public class Izdel
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
