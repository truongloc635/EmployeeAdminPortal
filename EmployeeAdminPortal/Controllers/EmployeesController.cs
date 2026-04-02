using EmployeeAdminPortal.Data;
using EmployeeAdminPortal.Models;
using EmployeeAdminPortal.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeAdminPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public EmployeesController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            var allEmployees = dbContext.Employees.ToList();
            return Ok(allEmployees);
        }
        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetEmployeeById(Guid id)
        {
            var employee = dbContext.Employees.Find(id);
            if (employee is null)
            {
                return NotFound();
            }
            return Ok(employee);
        }
        [HttpPut]
        [Route("{id:Guid}")]
        public IActionResult UpdateEmployee(Guid id, EmployeeVM employee)
        {
            var employeeEntity = dbContext.Employees.Find(id);
            if (employeeEntity is null)
            {
                return NotFound();
            }
            employeeEntity.Name = employee.Name;
            employeeEntity.Email = employee.Email;
            employeeEntity.Phone = employee.Phone;
            employeeEntity.Salary = employee.Salary;
            dbContext.SaveChanges();
            return Ok(employeeEntity);
        }
        [HttpPost]
        public IActionResult AddEmployee(EmployeeVM employee)
        {
            var employeeEntity = new Employee()
            {
                Name = employee.Name,
                Email = employee.Email,
                Phone = employee.Phone,
                Salary = employee.Salary
            };
            dbContext.Employees.Add(employeeEntity);
            dbContext.SaveChanges();
            return Ok(employeeEntity);
        }
        [HttpDelete]
        [Route("{id:Guid}")]
        public IActionResult DeleteEmployee(Guid id)
        {
            var employeeEntity = dbContext.Employees.Find(id);
            if (employeeEntity is null)
            {
                return NotFound();
            }
            dbContext.Employees.Remove(employeeEntity);
            dbContext.SaveChanges();
            return Ok(employeeEntity);
        }
    }
}
