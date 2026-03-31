using Microsoft.EntityFrameworkCore;
using RebelsClothing.Models.Entities;

namespace RebelsClothing.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}