using PharmacySystem.Server.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacySystem.Server.Models.ProducerModels.Response
{
    public class GetProducerListResponse
    {
        public List<ProducerItem> Items { get; set; }

        public PageResponse Page { get; set; }
    }
}
