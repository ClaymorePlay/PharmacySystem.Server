using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacySystem.Server.Models.Product
{
    public class GetProductsListResponse
    {
        public List<ProductListItem> Items { get; set; }
    }
}
