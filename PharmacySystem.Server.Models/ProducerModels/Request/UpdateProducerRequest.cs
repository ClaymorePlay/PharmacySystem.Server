using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacySystem.Server.Models.ProducerModels.Request
{
    public class UpdateProducerRequest
    {
        public int ProducerId { get; set; }

        public string? Name { get; set; }
    }
}
