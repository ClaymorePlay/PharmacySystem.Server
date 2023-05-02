using PharmacySystem.Server.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacySystem.Server.Models.ProductModels.Request
{
    public class GetProductsListRequest
    {
        public int? PharmacyId { get; set; }

        public string? ProductName { get; set; }

        public string? Description { get; set; }

        public int? ProducerId { get; set; }

        public PageRequest Page { get; set; }
    }
}
