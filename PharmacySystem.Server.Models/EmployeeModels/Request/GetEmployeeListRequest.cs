using PharmacySystem.Server.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacySystem.Server.Models.EmployeeModels.Request
{
    public class GetEmployeeListRequest
    {
        public string? Name { get; set; }

        public int? PharmacyId { get; set; }

        public PageRequest Page { get; set; }
    }
}
