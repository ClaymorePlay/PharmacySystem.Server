using Plugins.Pharmacy.IServices.Models.Request;
using Plugins.Pharmacy.IServices.Models.Response;
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
    
        Task<>
    }
}
