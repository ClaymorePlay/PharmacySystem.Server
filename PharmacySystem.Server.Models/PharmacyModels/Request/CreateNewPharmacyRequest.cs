using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacySystem.Server.Models.PharmacyModels.Request
{
    public class CreateNewPharmacyRequest
    {
        public string Adress { get; set; }

        public string Name { get; set; }

        public string Contacts { get; set; }
    }
}
