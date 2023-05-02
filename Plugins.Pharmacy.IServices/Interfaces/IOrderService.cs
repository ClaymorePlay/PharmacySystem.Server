using CodeEngine.WebSocket.Models.Schema;
using PharmacySystem.Server.Models.OrderModels.Request;
using PharmacySystem.Server.Models.OrderModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugins.Pharmacy.IServices.Interfaces
{
    public interface IOrderService
    {
        Task<OrderProductResponse> OrderProduct(OrderProductRequest request, RequestModel model); 
    }
}
