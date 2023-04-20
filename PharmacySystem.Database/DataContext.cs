using Microsoft.EntityFrameworkCore;
using PharmacySystem.Database.Entities;

namespace PharmacySystem.Database
{
    public class DataContext : DbContext
    {
        /// <summary>
        /// Аптеки
        /// </summary>
        public DbSet<Pharmacy> Pharmacies { get; set; }

        /// <summary>
        /// Продукты
        /// </summary>
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// Производители
        /// </summary>
        public DbSet<Producer> Producers { get; set; }

        /// <summary>
        /// Заказы
        /// </summary>
        public DbSet<Order> Orders { get; set; }

        /// <summary>
        /// Сотрудники
        /// </summary>
        public DbSet<Employee> Employees { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Pharmacy>().HasKey(c => c.Id);
            builder.Entity<Pharmacy>().HasMany(c => c.Products).WithOne(c => c.Pharmacy).HasForeignKey(c => c.PharmacyId);
            builder.Entity<Pharmacy>().HasMany(c => c.Orders).WithOne(c => c.Pharmacy).HasForeignKey(c => c.PharmacyId);
            builder.Entity<Pharmacy>().HasMany(c => c.Employees).WithOne(c => c.Pharmacy).HasForeignKey(c => c.PharmacyId);
        
        }
    }
}