using CodeEngine.WebSocket.Models.Schema;
using Microsoft.EntityFrameworkCore;
using PharmacySystem.Database;
using PharmacySystem.Database.Entities;
using PharmacySystem.Server.Models.OrderModels.Request;
using PharmacySystem.Server.Models.OrderModels.Response;
using Plugins.Pharmacy.IServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugins.Pharmacy.Services
{
    public class OrderService : IOrderService
    {
        private DbContextOptions<DataContext> _dbOptions { get; set; }

        public OrderService(DbContextOptions<DataContext> dbOptions)
        {
            _dbOptions = dbOptions;
        }

        /// <summary>
        /// Заказ
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<OrderProductResponse> OrderProduct(OrderProductRequest request, RequestModel model)
        {
            using (var db = new DataContext(_dbOptions))
            {
                var validProducts = await db.Products.Where(c => request.Items.Select(c => c.ProductId).Contains(c.Id) 
                    && c.Count >= request.Items.First(h => h.ProductId == c.Id).Count )
                    .ToListAsync();

                if (validProducts.Count != request.Items.Count)
                    return new OrderProductResponse { Ordered = false };

                var date = DateTime.Now;
                var orders = request.Items.Select(c => new Order
                {
                    DateOrder = date,
                    Count = c.Count,
                    ProductId = c.ProductId,
                    UserId = model.User.UserId,
                    ResultPrice = c.Count * validProducts.First(h => h.Id == c.ProductId).Price
                });

                await db.Orders.AddRangeAsync(orders.ToList());

                validProducts.ForEach(c =>
                {
                    c.Count -= request.Items.First(h => h.ProductId == c.Id).Count;
                });

                await db.SaveChangesAsync();
                return new OrderProductResponse { Ordered = true };
            }
        }
    }
}
