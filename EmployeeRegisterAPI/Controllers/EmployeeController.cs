using EmployeeRegisterAPI.Models;
using EmployeeRegisterAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EmployeeRegisterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public EmployeeController(IEmployeeService employeeService, IHttpContextAccessor httpContextAccessor)
        {
            _employeeService = employeeService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Route("[action]")]
        [Route("api/Employee/GetEmployees")]
        public IEnumerable<EmployeeModel> GetEmployees()
        {
            var Data = _employeeService.GetEmployees(_httpContextAccessor);
            return Data;
        }

        [HttpPost]
        [Route("[action]")]
        [Route("api/Employee/AddEmployee")]
        public IActionResult AddEmployee([FromForm]EmployeeModel employee)
        {
            employee.ImageName = SaveImage(employee.ImageFile);
            _employeeService.AddEmployee(employee);
            return Ok();
        }

        [HttpPost]
        [Route("[action]")]
        [Route("api/Employee/UpdateEmployee")]
        public IActionResult UpdateEmployee([FromForm]EmployeeModel employee)
        {
            if (employee.ImageFile != null)
            {
                string path = "Images/" + employee.ImageName;
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                employee.ImageName = SaveImage(employee.ImageFile);
            }
            _employeeService.UpdateEmployee(employee);
            return Ok();
        }

        [HttpDelete]
        [Route("[action]")]
        [Route("api/Employee/DeleteEmployee")]
        public IActionResult DeleteEmployee(int id)
        {
            var existingEmployee = _employeeService.GetEmployee(id);
            if (existingEmployee != null)
            {

                _employeeService.DeleteEmployee(existingEmployee.EmployeeId);
                string path = "Images/" + existingEmployee.ImageName;
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                return Ok();
            }
            return NotFound($"Employee Not Found with ID : {existingEmployee.EmployeeId}");
        }

        [HttpGet]
        [Route("GetEmployee")]
        public EmployeeModel GetEmployee(int id)
        {
            return _employeeService.GetEmployee(id);
        }
        [NonAction]
        public string SaveImage(IFormFile imageFile)
        {
            string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');
            imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(imageFile.FileName);
            var imagePath = "Images/" + imageName;
            // var fileName = Path.Combine(Server.MapPath("Images/"), imageName);
            using (var fileStream = new FileStream(imagePath,FileMode.Create))
            {
                imageFile.CopyTo(fileStream);
            }
            return imageName;
           
        }
    }
}
