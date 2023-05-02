﻿using Microsoft.EntityFrameworkCore;
using PharmacySystem.Database;
using PharmacySystem.Database.Entities;
using PharmacySystem.Server.Models.EmployeeModels.Request;
using PharmacySystem.Server.Models.EmployeeModels.Response;
using Plugins.Pharmacy.IServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugins.Pharmacy.Services
{
    public class EmployeeService : IEmployeeService
    {
        private DbContextOptions<DataContext> _dbOptions { get; set; }

        public EmployeeService(DbContextOptions<DataContext> dbOptions)
        {
            _dbOptions = dbOptions;
        }

        /// <summary>
        /// Добавление сотрудника
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<CreateEmployeeResponse> CreateEmployee(CreateEmployeeRequest request)
        {
            using (var db = new DataContext(_dbOptions))
            {
                var employee = new Employee
                {
                    DateStart = DateTime.Now,
                    Salary = request.Salary,
                    FullName = request.FullName,
                    Gender = request.Gender,
                    PharmacyId = request.PharmacyId,
                };

                await db.AddAsync(employee);

                await db.SaveChangesAsync();
                return new CreateEmployeeResponse { Id = employee.Id };
            }
        }

        /// <summary>
        /// Удаление сотрудника
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<RemoveEmployeeResponse> RemoveEmployee(RemoveEmployeeRequest request)
        {
            using (var db = new DataContext(_dbOptions))
            {
                if (await db.Employees.AnyAsync(c => c.Id == request.Id))
                    return new RemoveEmployeeResponse { Removed = false };

                db.Employees.Remove(new Employee { Id = request.Id });
                await db.SaveChangesAsync();

                return new RemoveEmployeeResponse { Removed = true };

            }
        }

        /// <summary>
        /// Обновление сотрудника
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<UpdateEmployeeResponse> UpdateEmployee(UpdateEmployeeRequest request)
        {
            using (var db = new DataContext(_dbOptions))
            {
                var employee = await db.Employees.FirstOrDefaultAsync(c => c.Id == request.Id);

                if (employee == null)
                    return new UpdateEmployeeResponse { Updated = false };

                employee.FullName = request.FullName ?? employee.FullName;
                employee.Gender = request.Gender ?? employee.Gender;
                employee.PharmacyId = request.PharmacyId ?? employee.PharmacyId;
                employee.Salary = request.Salary ?? employee.Salary;

                await db.SaveChangesAsync();
                return new UpdateEmployeeResponse { Updated = true };
            }
        }
    }
}