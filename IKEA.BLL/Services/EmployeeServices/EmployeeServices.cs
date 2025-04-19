using IKEA.BLL.Dto_s.Employees;
using IKEA.DAL.Models.Employees;
using IKEA.DAL.Persistance.Repositories.Employees;
using Microsoft.EntityFrameworkCore;

namespace IKEA.BLL.Services.EmployeeServices
{
    public class EmployeeServices:IEmployeeServices
    {
        private readonly IEmployeeRepositoris repository;
        public EmployeeServices(IEmployeeRepositoris employeeRepository) => repository = employeeRepository;
        public IEnumerable<EmployeeDto> GetAllEmployees(string search) =>
            repository.GetAll().Where(E => !E.IsDeleted &&  (string.IsNullOrEmpty(search) || E.Name.ToLower().Contains(search.ToLower()))).Include(E => E.Department).Select(E => new EmployeeDto()
            {
                Id = E.Id,
                Name = E.Name,
                Age = E.Age,
                Salary = E.Salary,
                IsActive = E.IsActive,
                Email = E.Email,
                Gender = E.Gender,
                EmployeeType = E.EmployeeType,
                Department = E.Department .Name ?? "N/A"
            }).ToList();


        public EmployeeDetailsDto? GetEmployeeById(int id)
        {
            var E = repository.GetById(id);
            if (E is not null)
                return new EmployeeDetailsDto()
                    {
                       Id = E.Id,
                       Name = E.Name,
                       Age = E.Age,
                       Address = E.Address,
                       IsActive = E.IsActive,
                       Salary = E.Salary,
                       Email = E.Email,
                       PhoneNumber = E.PhoneNumber,
                       HiringDate = E.HiringDate,
                       Gender = E.Gender,
                       EmployeeType = E.EmployeeType,
                       LastModifiedBy = E.LastModifiedBy,
                       CreatedBy = E.CreatedBy,
                       LastModifiedOn = E.LastModifiedOn,
                       CreatedOn = E.CreatedOn,
                       Department = E.Department?.Name ?? "N/A"
                };
            return null;
        }

        public int CreateEmployee(CreatedEmployeeDto employeeDto) =>
            repository.Add(new Employee()
            {
                Name = employeeDto.Name,
                Age = employeeDto.Age,
                Address = employeeDto.Address,
                IsActive = employeeDto.IsActive,
                Salary = employeeDto.Salary,
                Email = employeeDto.Email,
                PhoneNumber = employeeDto.PhoneNumber,
                Gender = employeeDto.Gender,
                EmployeeType = employeeDto.EmployeeType,
                DepartmentId = employeeDto.DepartmentId,
                CreatedBy = 1,
                LastModifiedBy = 1,
                LastModifiedOn = DateTime.Now,
                CreatedOn = DateTime.Now,

            });
     
        public int UpdateEmployee(UpdatedEmployeeDto employeeDto) =>
            repository.Update(new Employee()
            {
                Id = employeeDto.Id,
                Name = employeeDto.Name,
                Age = employeeDto.Age,
                Address = employeeDto.Address,
                IsActive = employeeDto.IsActive,
                Salary = employeeDto.Salary,
                Email = employeeDto.Email,
                PhoneNumber = employeeDto.PhoneNumber,
                HiringDate = employeeDto.HiringDate,
                Gender = employeeDto.Gender,
                EmployeeType = employeeDto.EmployeeType,
                DepartmentId = employeeDto.DepartmentId,
                LastModifiedBy = 1,
                LastModifiedOn = DateTime.Now,
            });

        public bool DeleteEmployee(int id)
        => repository.GetById(id) is not null ? repository.Delete(repository.GetById(id)) > 0 : false;


    }
}
