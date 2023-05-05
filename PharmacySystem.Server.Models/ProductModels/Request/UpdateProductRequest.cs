using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacySystem.Server.Models.ProductModels.Request
{
    public class UpdateProductRequest
    {
        public long ProductId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public int? Count { get; set; }

        public decimal? Price { get; set; }
    }
}
