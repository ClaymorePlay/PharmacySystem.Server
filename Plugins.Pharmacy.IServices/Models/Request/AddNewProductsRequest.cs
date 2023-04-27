using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugins.Pharmacy.IServices.Models.Request
{
    public class AddNewProductsRequest
    {
        public List<NewProduct> Products { get; set; }
    }
}

