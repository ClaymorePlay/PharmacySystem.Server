using CodeEngine.WebSocket.Models.Schema;
using PharmacySystem.Server.Models.PharmacyModels.Request;
using PharmacySystem.Server.Models.PharmacyModels.Response;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugins.Pharmacy.IServices.Interfaces
{
    public interface IPharmacyService
    {
        Task<CreateNewPharmacyResponse> CreateNewPharmacy(CreateNewPharmacyRequest request, RequestModel user);

        Task<GetPharmacyListResponse> GetPharmacyList(GetPharmacyListRequest request);

        Task<RemovePharmacyResponse> RemovePharmacy(RemovePharmacyRequest request, RequestModel user);

        Task<UpdatePharmacyResponse> UpdatePharmacy(UpdatePharmacyRequest request, RequestModel user);
    }
}
