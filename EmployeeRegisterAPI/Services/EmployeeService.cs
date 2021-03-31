using EmployeeRegisterAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Features;

namespace EmployeeRegisterAPI.Services
{
    public class EmployeeService : IEmployeeService
    {
        public EmployeeDbContext _employeeDbContext;
        public EmployeeService(EmployeeDbContext employeeDbContext)
        {
            _employeeDbContext = employeeDbContext;
        }
        public EmployeeModel AddEmployee(EmployeeModel employee)
        {
            _employeeDbContext.Employees.Add(employee);
            _employeeDbContext.SaveChanges();
            return employee;
        }
        public List<EmployeeModel> GetEmployees(IHttpContextAccessor httpContextAccessor)
        {
            return _employeeDbContext.Employees.Select(x => new EmployeeModel() { 
            EmployeeId = x.EmployeeId,
            EmployeeName = x.EmployeeName,
            Occupation = x.Occupation,
            ImageName = x.ImageName,
            ImageSrc = String.Format("{0}://{1}{2}/Images/{3}",httpContextAccessor.HttpContext.Request.Scheme, httpContextAccessor.HttpContext.Request.Host, httpContextAccessor.HttpContext.Request.PathBase,x.ImageName)
            }).ToList();
        }

        public void UpdateEmployee(EmployeeModel employee)
        {
            _employeeDbContext.Employees.Update(employee);
            _employeeDbContext.SaveChanges();
        }

        public void DeleteEmployee(int Id)
        {
            var employee = _employeeDbContext.Employees.FirstOrDefault(x => x.EmployeeId == Id);
            if (employee != null)
            {
                _employeeDbContext.Remove(employee);
                _employeeDbContext.SaveChanges();
            }
        }

        public EmployeeModel GetEmployee(int Id)
        {
            return _employeeDbContext.Employees.FirstOrDefault(x => x.EmployeeId == Id);
        }
       
    }
}
