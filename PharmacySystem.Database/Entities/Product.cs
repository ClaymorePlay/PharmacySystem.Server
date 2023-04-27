using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacySystem.Database.Entities
{
    public class Product
    {
        public long Id { get; set; }

        public decimal Price { get; set; }

        public string Name { get; set; }
 
        public int ProducerId { get; set; }

        public int PharmacyId { get; set; }

        public int Count { get; set; }

        public Producer Producer { get; set; }

        public Pharmacy Pharmacy { get; set; }

        public List<Order> Orders { get; set; }


    }
}
