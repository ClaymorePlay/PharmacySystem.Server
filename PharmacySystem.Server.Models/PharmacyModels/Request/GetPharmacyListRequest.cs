using PharmacySystem.Server.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacySystem.Server.Models.PharmacyModels.Request
{
    public class GetPharmacyListRequest
    {
        public PageRequest Page { get; set; }

        public string? Name { get; set; }
    }
}
