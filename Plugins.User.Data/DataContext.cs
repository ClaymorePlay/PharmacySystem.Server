using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GaneshaProgramming.Plugins.User.Data
{
    public class DataContext : DbContext
    {
        


        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        {
            //Database.EnsureCreated();
        }

        public DbSet<Data.Models.User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.User>(c => c.HasKey(x => x.Id));
            //modelBuilder.Entity<Models.User>().HasData(new GaneshaProgramming.Plugins.User.Data.Models.User
            //{
            //    Id = 1,
            //    Email = "kacher-2005@bk.ru",
            //    EmailConfirm = true,
            //    Password = ComputeSha256Hash("admin"),
            //    AutToken = Guid.NewGuid(),
            //    Role = IServices.Models.Enum.RoleEnum.Admin,
            //    UserName = "Данила Кучер"
            //});
            
        }

        private string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }

}
