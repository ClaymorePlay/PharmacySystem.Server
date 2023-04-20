using GaneshaProgramming.Identity.Models;
using Microsoft.EntityFrameworkCore;
 
namespace GaneshaProgramming.Identity
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.Migrate();
        }
    }
}