using CodeEngine.WebSocket.Models.Schema;
using Microsoft.EntityFrameworkCore;
using PharmacySystem.Database;
using PharmacySystem.Server.Models.PharmacyModels.Request;
using PharmacySystem.Server.Models.PharmacyModels.Response;
using Plugins.Pharmacy.IServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugins.Pharmacy.Services
{
    public class PharmacyService : IPharmacyService
    {
        private DbContextOptions<DataContext> _dbOptions { get; set; }

        public PharmacyService(DbContextOptions<DataContext> dbOptions)
        {
            _dbOptions = dbOptions;
        }

        /// <summary>
        /// Создание аптеки
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<CreateNewPharmacyResponse> CreateNewPharmacy(CreateNewPharmacyRequest request, RequestModel user)
        {
            if (user.User.Role != GaneshaProgramming.Plugins.User.IServices.Models.Enum.RoleEnum.Admin)
                throw new Exception("У вас не достаточно прав на внесение изменений");

            using (var db = new DataContext(_dbOptions))
            {
                if(await db.Pharmacies.AnyAsync(c => c.Name.ToLower().Trim() == request.Name.ToLower().Trim() 
                    && c.Adress.Trim().ToLower() == request.Adress.ToLower().Trim()))
                {
                    throw new Exception("Аптека с таким названием и адресом уже добавлена");
                }

                var pharmacy = new PharmacySystem.Database.Entities.Pharmacy
                {
                    Name = request.Name,
                    Adress = request.Adress,
                    Contacts = request.Contacts
                };

                await db.Pharmacies.AddAsync(pharmacy);

                await db.SaveChangesAsync();
                return new CreateNewPharmacyResponse { Id = pharmacy.Id };
            }
        }

        /// <summary>
        /// Получить список аптек
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<GetPharmacyListResponse> GetPharmacyList(GetPharmacyListRequest request)
        {
            using (var db = new DataContext(_dbOptions))
            {
                var pharmacies = db.Pharmacies.AsQueryable();

                if (!String.IsNullOrWhiteSpace(request.Name))
                    pharmacies = pharmacies.Where(c => c.Name.Contains(request.Name));

                var count = await pharmacies.CountAsync();
                var response = await pharmacies.Skip(request.Page.Skip).Take(request.Page.Take).Select(c => new PharmacyItem
                {
                    Id = c.Id,
                    Adress = c.Adress,
                    Contact = c.Contacts,
                    Name = c.Name
                }).ToListAsync();

                return new GetPharmacyListResponse
                {
                    PharmacyList = response,
                    Page = new PharmacySystem.Server.Models.Common.PageResponse
                    {
                        Count = count,
                        Size = response.Count,
                        Current = (request.Page.Skip + request.Page.Take) / request.Page.Take
                    }
                };
            }
        }

        /// <summary>
        /// Удаление аптеки
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<RemovePharmacyResponse> RemovePharmacy(RemovePharmacyRequest request, RequestModel user)
        {
            if (user.User.Role != GaneshaProgramming.Plugins.User.IServices.Models.Enum.RoleEnum.Admin)
                throw new Exception("У вас не достаточно прав на внесение изменений");

            using (var db = new DataContext(_dbOptions))
            {
                if (!await db.Pharmacies.AnyAsync(c => c.Id == request.PharmacyId))
                    return new RemovePharmacyResponse() { IsRemoved = false };

                db.Pharmacies.Remove(new PharmacySystem.Database.Entities.Pharmacy { Id = request.PharmacyId });
                await db.SaveChangesAsync();

                return new RemovePharmacyResponse() { IsRemoved = true };
            }
        }

        /// <summary>
        /// Обновление аптеки
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<UpdatePharmacyResponse> UpdatePharmacy(UpdatePharmacyRequest request, RequestModel user)
        {
            if (user.User.Role != GaneshaProgramming.Plugins.User.IServices.Models.Enum.RoleEnum.Admin)
                throw new Exception("У вас не достаточно прав на внесение изменений");

            using (var db = new DataContext(_dbOptions))
            {
                var pharmacy = await db.Pharmacies.FirstOrDefaultAsync();

                if (pharmacy == null)
                    return new UpdatePharmacyResponse { IsUpdated = false };

                pharmacy.Name = request.Name ?? pharmacy.Name;
                pharmacy.Adress = request.Adress ?? pharmacy.Adress;
                pharmacy.Contacts = request.Contacts ?? pharmacy.Contacts;

                await db.SaveChangesAsync();
                return new UpdatePharmacyResponse { IsUpdated = true };
            }
        }
    }
}
