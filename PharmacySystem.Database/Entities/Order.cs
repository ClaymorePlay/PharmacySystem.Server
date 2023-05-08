using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacySystem.Database.Entities
{
    public class Order
    {
        public Guid Id { get; set; }

        public long ProductId { get; set; }

        public DateTime DateOrder { get; set; }

        public long UserId { get; set; }

        public int Count { get; set; }

        public decimal ResultPrice { get; set; }

        public Product Product { get; set;  }
    }
}
