using CodeEngine.WebSocket.Models.Schema;
using Microsoft.EntityFrameworkCore;
using PharmacySystem.Database;
using PharmacySystem.Database.Entities;
using PharmacySystem.Server.Models.Product;
using PharmacySystem.Server.Models.ProductModels.Request;
using PharmacySystem.Server.Models.ProductModels.Response;
using Plugins.Pharmacy.IServices.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugins.Pharmacy.Services
{
    public class ProductService : IProductService
    {
        private DbContextOptions<DataContext> _dbOptions { get; set; }

        public ProductService(DbContextOptions<DataContext> dbOptions)
        {
            _dbOptions = dbOptions;
        }

        /// <summary>
        /// Повышение кол-ва товара
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<AddNewProductsResponse> AddNewProducts(AddNewProductsRequest request, RequestModel user)
        {
            if (user.User.Role != GaneshaProgramming.Plugins.User.IServices.Models.Enum.RoleEnum.Admin)
                throw new Exception("У вас не достаточно прав на внесение изменений");


            using (var db = new DataContext(_dbOptions))
            {
                var proudcts = await db.Products.Where(c => request.Products.Select(h => h.ProductId).Contains(c.Id)).ToListAsync();

                if (proudcts == null || proudcts.Count == 0)
                    return new AddNewProductsResponse { IsAdded = false };

                proudcts.ForEach(c =>
                {
                    c.Count += request.Products.First(h => h.ProductId == c.Id).Count;
                });

                await db.SaveChangesAsync();
                return new AddNewProductsResponse { IsAdded = true };
            }
        }

        /// <summary>
        /// Создание товара
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<AddNewProductsResponse> CreateProduct(CreateProductRequest request, RequestModel user)
        {
            if (user.User.Role != GaneshaProgramming.Plugins.User.IServices.Models.Enum.RoleEnum.Admin)
                throw new Exception("У вас не достаточно прав на внесение изменений");


            using (var db = new DataContext(_dbOptions))
            {
                if (db.Products.Any(c => c.Name.ToLower().Trim() == request.Name.ToLower().Trim()))
                    return new AddNewProductsResponse() { IsAdded = false };

                var product = new Product
                {
                    Description = request.Description,
                    Count = request.Count,
                    Name = request.Name,
                    PharmacyId = request.PharmacyId,
                    Price = request.Price,
                    ProducerId = request.ProducerId,
                };

                await db.Products.AddAsync(product);
                await db.SaveChangesAsync();

                return new AddNewProductsResponse() { IsAdded = true };
            }
        }

        /// <summary>
        /// Получение списка продуктов
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<GetProductsListResponse> GetProducts(GetProductsListRequest request)
        {
            using (var db = new DataContext(_dbOptions))
            {
                var products = db.Products.AsQueryable();

                if (!String.IsNullOrWhiteSpace(request.ProductName))
                    products = products.Where(c => c.Name.Contains(request.ProductName));

                if (!String.IsNullOrWhiteSpace(request.Description))
                    products = products.Where(c => c.Description.Contains(request.Description));

                if (request.ProducerId.HasValue)
                    products = products.Where(c => c.ProducerId == request.ProducerId.Value);
            
                if (request.PharmacyId.HasValue)
                    products = products.Where(c => c.PharmacyId == request.PharmacyId.Value);

                var count = await products.CountAsync();
                var response = await products
                    .Skip(request.Page.Skip).Take(request.Page.Take)
                    .Select(c => new ProductListItem
                    {
                        PharmacyId = c.PharmacyId,
                        Count = c.Count,
                        PharmacyName = c.Pharmacy.Name,
                        Price = c.Price,
                        ProducerName = c.Producer.Name,
                        ProductId = c.Id,
                        ProductName = c.Name,
                        Description = c.Description
                    })
                    .ToListAsync();

                return new GetProductsListResponse 
                { 
                    Items = response,
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
        /// Удаление товара
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<RemoveProductResponse> RemoveProduct(RemoveProductRequest request, RequestModel user)
        {
            if (user.User.Role != GaneshaProgramming.Plugins.User.IServices.Models.Enum.RoleEnum.Admin)
                throw new Exception("У вас не достаточно прав на внесение изменений");


            using (var db = new DataContext(_dbOptions))
            {
                var product = await db.Products.FirstOrDefaultAsync(c => c.Id == request.ProductId);
                if (product == null)
                    return new RemoveProductResponse { Removed = false };

                db.Products.Remove(product);
                await db.SaveChangesAsync();

                return new RemoveProductResponse { Removed = true };
            }
        }

        /// <summary>
        /// Обновление товара
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<UpdateProductResponse> UpdateProduct(UpdateProductRequest request, RequestModel user)
        {
            if (user.User.Role != GaneshaProgramming.Plugins.User.IServices.Models.Enum.RoleEnum.Admin)
                throw new Exception("У вас не достаточно прав на внесение изменений");


            using (var db = new DataContext(_dbOptions))
            {
                var product = await db.Products.FirstOrDefaultAsync(c => c.Id == request.ProductId);

                if (product == null)
                    return new UpdateProductResponse { Updated = false };

                product.Name = request.Name ?? product.Name;
                product.Price = request.Price ?? product.Price;
                product.Count = request.Count ?? product.Count;
                product.Description = request.Description ?? product.Description;

                await db.SaveChangesAsync();

                return new UpdateProductResponse { Updated = true };
            }
        }

        public async Task<ProductListItem> GetById(GetProductByIdRequest request)
        {
            using (var db = new DataContext(_dbOptions))
            {
                var product = await db.Products.FirstOrDefaultAsync(c => c.Id == request.PrioductId);

                if (product == null)
                    return new ProductListItem();

                return new ProductListItem
                {
                    PharmacyId = product.PharmacyId,
                    Count = product.Count,
                    Description = product.Description,
                    Price = product.Price,
                    ProductId = product.Id,
                    ProductName = product.Name,
                };
            }
        }
    }
}
