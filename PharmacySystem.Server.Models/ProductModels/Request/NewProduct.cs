using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacySystem.Server.Models.Product
{
    public class NewProduct
    {
        /// <summary>
        /// Номер продукта
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// Количество
        /// </summary>
        public int Count { get; set; }
    }
}
