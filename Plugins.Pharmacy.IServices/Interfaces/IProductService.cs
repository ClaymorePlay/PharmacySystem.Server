using PharmacySystem.Server.Models.Product;
using PharmacySystem.Server.Models.ProductModels.Request;
using PharmacySystem.Server.Models.ProductModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugins.Pharmacy.IServices.Interfaces
{
    public interface IProductService
    {
        Task<AddNewProductsResponse> AddNewProducts(AddNewProductsRequest request);

        Task<AddNewProductsResponse> CreateProduct(CreateProductRequest request);
    
        Task<GetProductsListResponse> GetProducts(GetProductsListRequest request);

        Task<RemoveProductResponse> RemoveProduct(RemoveProductRequest request);
    }
}
