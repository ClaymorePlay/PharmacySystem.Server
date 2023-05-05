using Microsoft.EntityFrameworkCore;
using PharmacySystem.Database;
using PharmacySystem.Database.Entities;
using PharmacySystem.Server.Models.ProducerModels.Request;
using PharmacySystem.Server.Models.ProducerModels.Response;
using Plugins.Pharmacy.IServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugins.Pharmacy.Services
{
    public class ProducerService : IProducerService
    {
        private DbContextOptions<DataContext> _dbOptions { get; set; }

        public ProducerService(DbContextOptions<DataContext> dbOptions)
        {
            _dbOptions = dbOptions;
        }

        /// <summary>
        /// Создание производителя
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<CreateProducerResponse> CreateProducer(CreateProducerRequest request)
        {
            using (var db = new DataContext(_dbOptions))
            {
                if (await db.Producers.AnyAsync(c => c.Name.ToLower().Trim() == request.Name.ToLower().Trim()))
                    return new CreateProducerResponse { Id = 0 };

                var producer = new Producer { Name = request.Name };
                await db.Producers.AddAsync(producer);

                await db.SaveChangesAsync();
                return new CreateProducerResponse { Id = producer.Id };
            }
        }

        /// <summary>
        /// Удаление производителя
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<RemoveProducerResponse> RemoveProducer(RemoveProducerRequest request)
        {
            using (var db = new DataContext(_dbOptions))
            {
                var producer = await db.Producers.FirstOrDefaultAsync(c => c.Id == request.Id);
                if (producer == null)
                    return new RemoveProducerResponse { Removed = false };

                db.Producers.Remove(producer);

                await db.SaveChangesAsync();
                return new RemoveProducerResponse { Removed = true };
            }
        }

        /// <summary>
        /// Обновление производителя
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<UpdateProducerResponse> UpdateProducer(UpdateProducerRequest request)
        {
            using (var db = new DataContext(_dbOptions))
            {
                var producer = await db.Producers.FirstOrDefaultAsync(c => c.Id == request.ProducerId);

                if (producer == null)
                    return new UpdateProducerResponse { Updated = false };

                producer.Name = request.Name ?? producer.Name;

                await db.SaveChangesAsync();
                return new UpdateProducerResponse { Updated = true };
            }
        }

        /// <summary>
        /// Получение производителей
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<GetProducerListResponse> GetProducerList(GetProducerListRequest request)
        {
            using (var db = new DataContext(_dbOptions))
            {
                var producers = db.Producers.AsQueryable();

                if (!String.IsNullOrWhiteSpace(request.Name))
                    producers = producers.Where(c => c.Name.ToLower().Contains(request.Name.ToLower()));

                var count = await producers.CountAsync();
                var result = await producers.Skip(request.Page.Skip).Take(request.Page.Take).ToListAsync();

                return new GetProducerListResponse
                {
                    Items = result.Select(c => new ProducerItem { Id = c.Id, Name = c.Name }).ToList(),
                    Page = new PharmacySystem.Server.Models.Common.PageResponse
                    {
                        Count = count,
                        Size = result.Count,
                        Current = (request.Page.Skip + request.Page.Take) / request.Page.Take
                    }
                };
            }
        }
    }
}
