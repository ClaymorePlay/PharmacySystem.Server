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
        Task<CreateProducerResponse> CreateProducer(CreateProducerRequest request);

        Task<RemoveProducerResponse> RemoveProducer(RemoveProducerRequest request);

        Task<UpdateProducerResponse> UpdateProducer(UpdateProducerRequest request);

        Task<GetProducerListResponse> GetProducerList(GetProducerListRequest request);
    }
}
