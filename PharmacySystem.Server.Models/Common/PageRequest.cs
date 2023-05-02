using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacySystem.Server.Models.Common
{
    public class PageRequest
    {
        public int Take { get; set; }

        public int Skip { get; set; }
    }
}
