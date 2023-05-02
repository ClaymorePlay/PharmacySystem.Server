using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacySystem.Server.Models.OrderModels.Request
{
    public class OrderProductItem
    {
        public long ProductId { get; set; }

        public int Count { get; set; }
    }
}
