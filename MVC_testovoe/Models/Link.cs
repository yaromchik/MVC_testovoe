using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MVC_testovoe.Models
{
    public class Link
    {
        [Key]
        public long Id { get; set; }
        public long IzdelUpId { get; set; }
        [ForeignKey("IzdelUpId")]
        public Izdel IzdelUp { get; set; }
        public long IzdelId { get; set; }
        [ForeignKey("IzdelId")]
        public Izdel Izdel { get; set; }
        public int kol { get; set; }
    }
}
