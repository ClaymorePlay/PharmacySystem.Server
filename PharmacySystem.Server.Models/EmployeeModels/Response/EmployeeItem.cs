using PharmacySystem.Database.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacySystem.Server.Models.EmployeeModels.Response
{
    public class EmployeeItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Salary { get; set; }

        public GenderEnum Gender { get; set; }

        public int PharmacyId { get; set; }
    }
}
