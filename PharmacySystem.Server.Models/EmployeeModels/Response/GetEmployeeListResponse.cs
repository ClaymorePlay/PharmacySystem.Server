using PharmacySystem.Server.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacySystem.Server.Models.EmployeeModels.Response
{
    public class GetEmployeeListResponse
    {
        public PageResponse Page { get; set; }

        public List<EmployeeItem> Items { get; set; }
    }
}
