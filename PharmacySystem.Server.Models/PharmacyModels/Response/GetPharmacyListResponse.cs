using PharmacySystem.Server.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacySystem.Server.Models.PharmacyModels.Response
{
    public class GetPharmacyListResponse
    {
        public List<PharmacyItem> PharmacyList { get; set; }

        public PageResponse Page { get; set; }
    }
}
