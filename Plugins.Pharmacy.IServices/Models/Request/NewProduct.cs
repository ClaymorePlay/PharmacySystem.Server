using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugins.Pharmacy.IServices.Models.Request
{
    public class NewProduct
    {
        /// <summary>
        /// Номер продукта
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// Номер аптеки
        /// </summary>
        public int PharmacyId { get; set; }

        /// <summary>
        /// Количество
        /// </summary>
        public int Count { get; set; }
    }
}
