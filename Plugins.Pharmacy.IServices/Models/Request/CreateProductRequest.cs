using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugins.Pharmacy.IServices.Models.Request
{
    public class CreateProductRequest
    {
        public string Name { get; set; }
        
        public decimal Price { get; set; }

        public int PharmacyId { get; set; }

        public int Count { get; set; }

        public int ProducerId { get; set; }
    }
}
