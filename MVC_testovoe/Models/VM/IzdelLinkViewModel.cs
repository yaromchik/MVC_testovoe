using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace MVC_testovoe.Models.VM
{
    public class IzdelLinkViewModel
    {
        public List<Izdel> Izdels { get; set; }
        public List<Link> Links { get; set; }
        public Izdel NewIzdel { get; set; }
        public Link NewLink { get; set; }
    }
}
