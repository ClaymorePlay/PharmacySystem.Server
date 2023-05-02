using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacySystem.Server.Models.Common
{
    public class PageResponse
    {
        public int Size { get; set; }

        public int Current { get; set; }

        public int Count { get; set; }
    }
}
