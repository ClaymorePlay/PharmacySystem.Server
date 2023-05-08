using CodeEngine.WebSocket.Models.Schema;
using PharmacySystem.Server.Models.ProducerModels.Request;
using PharmacySystem.Server.Models.ProducerModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugins.Pharmacy.IServices.Interfaces
{
    public interface IProducerService
    {
        Task<CreateProducerResponse> CreateProducer(CreateProducerRequest request, RequestModel user);

        Task<RemoveProducerResponse> RemoveProducer(RemoveProducerRequest request, RequestModel user);

        Task<UpdateProducerResponse> UpdateProducer(UpdateProducerRequest request, RequestModel user);

        Task<GetProducerListResponse> GetProducerList(GetProducerListRequest request);
    }
}
