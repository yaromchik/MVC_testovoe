using Microsoft.EntityFrameworkCore;
using MVC_testovoe.Models;

namespace MVC_testovoe.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() { }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Izdel> Izdels { get; set; }
        public DbSet<Link> Links { get; set; }

        
    }
}
