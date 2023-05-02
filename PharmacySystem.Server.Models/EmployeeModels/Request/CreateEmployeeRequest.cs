using PharmacySystem.Database.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacySystem.Server.Models.EmployeeModels.Request
{
    public class CreateEmployeeRequest
    {
        public GenderEnum Gender { get; set; }

        public decimal Salary { get; set; }

        public string FullName { get; set; }

        public int PharmacyId { get; set; }
    }
}
