using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugins.Pharmacy.IServices.Models.Response
{
    public class GetProductsListResponse
    {
        public List<ProductListItem> Items { get; set; }
    }
}
