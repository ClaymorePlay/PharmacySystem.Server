using PharmacySystem.Database.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacySystem.Database.Entities
{
    public class Employee
    {
        public int Id { get; set; }

        public GenderEnum Gender { get; set; }

        public DateTime DateStart { get; set; }

        public string FullName { get; set; }

        public decimal Salary { get; set; }

        public int PharmacyId { get; set; }

        public Pharmacy Pharmacy { get; set; }
    }
}
