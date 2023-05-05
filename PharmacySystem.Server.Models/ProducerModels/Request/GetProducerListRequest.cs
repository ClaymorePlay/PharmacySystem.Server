using PharmacySystem.Server.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacySystem.Server.Models.ProducerModels.Request
{
    public class GetProducerListRequest
    {
        public string? Name { get; set; }

        public PageRequest Page { get; set; }
    }
}
