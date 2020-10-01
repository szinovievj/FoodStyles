using FoodStyles.Utils;
using Microsoft.EntityFrameworkCore;

namespace FoodStyles.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            
        }
        
        public DbSet<MenuItem> Foods { get; set; }
    }
}