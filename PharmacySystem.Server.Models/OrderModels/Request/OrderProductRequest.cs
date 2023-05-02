using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacySystem.Server.Models.OrderModels.Request
{
    public class OrderProductRequest
    {
        public List<OrderProductItem> Items { get; set; }
    }
}
