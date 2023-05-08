using CodeEngine.WebSocket.Models.Schema;
using PharmacySystem.Server.Models.EmployeeModels.Request;
using PharmacySystem.Server.Models.EmployeeModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugins.Pharmacy.IServices.Interfaces
{
    public interface IEmployeeService
    {
        Task<CreateEmployeeResponse> CreateEmployee(CreateEmployeeRequest request, RequestModel user);

        Task<RemoveEmployeeResponse> RemoveEmployee(RemoveEmployeeRequest request, RequestModel user);

        Task<UpdateEmployeeResponse> UpdateEmployee(UpdateEmployeeRequest request, RequestModel user);

        Task<GetEmployeeListResponse> GetEmployeesList(GetEmployeeListRequest request, RequestModel user);
    }
}
