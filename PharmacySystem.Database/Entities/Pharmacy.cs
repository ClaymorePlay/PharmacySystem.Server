﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacySystem.Database.Entities
{
    public class Pharmacy
    {
        /// <summary>
        /// Номер записи
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public string Adress { get; set; }

        public List<Employee> Employees { get; set; }

        public List<Product> Products { get; set; }

        public List<Order> Orders { get; set; }
    }
}