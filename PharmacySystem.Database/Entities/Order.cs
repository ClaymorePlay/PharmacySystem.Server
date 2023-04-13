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

        public int PharmacyId { get; set; }

        public DateTime DateOrder { get; set; }

        public Pharmacy Pharmacy { get; set; }

        public Product Product { get; set;  }
    }
}
