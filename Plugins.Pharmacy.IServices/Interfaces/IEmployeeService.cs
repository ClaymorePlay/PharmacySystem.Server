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
        Task<CreateEmployeeResponse> CreateEmployee(CreateEmployeeRequest request);

        Task<RemoveEmployeeResponse> RemoveEmployee(RemoveEmployeeRequest request);

        Task<UpdateEmployeeResponse> UpdateEmployee(UpdateEmployeeRequest request);

        Task<GetEmployeeListResponse> GetEmployeesList(GetEmployeeListRequest request);
    }
}
