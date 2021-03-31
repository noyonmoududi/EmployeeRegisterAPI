using EmployeeRegisterAPI.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeRegisterAPI.Services
{
    public interface IEmployeeService
    {
        EmployeeModel AddEmployee(EmployeeModel employee);

        List<EmployeeModel> GetEmployees(IHttpContextAccessor httpContextAccessor);

        void UpdateEmployee(EmployeeModel employee);

        void DeleteEmployee(int Id);

        EmployeeModel GetEmployee(int Id);
    }
}
