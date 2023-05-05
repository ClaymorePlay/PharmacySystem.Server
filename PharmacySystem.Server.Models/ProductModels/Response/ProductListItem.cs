using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacySystem.Server.Models.Product
{
    public class ProductListItem
    {
        public long ProductId { get; set; }

        public string PharmacyName { get; set; }

        public int PharmacyId { get; set; }

        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public string ProducerName { get; set; }

        public int Count { get; set; }

        public string Description { get; set; }
    }
}
