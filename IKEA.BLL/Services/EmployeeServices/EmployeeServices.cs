using IKEA.BLL.Dto_s.Employees;
using IKEA.DAL.Models.Employees;
using IKEA.DAL.Persistance.Repositories.Employees;
using IKEA.DAL.Persistance.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace IKEA.BLL.Services.EmployeeServices
{
    public class EmployeeServices:IEmployeeServices
    {
        
        private readonly IUnitOfWork unitOfWork;

        public EmployeeServices(IUnitOfWork unitOfWork)
        {;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<EmployeeDto> GetAllEmployees(string search) =>
            unitOfWork.employeeRepositoris.GetAll().Where(E => !E.IsDeleted &&  (string.IsNullOrEmpty(search) || E.Name.ToLower().Contains(search.ToLower()))).Include(E => E.Department).Select(E => new EmployeeDto()
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
            var E = unitOfWork.employeeRepositoris.GetById(id);
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

        public int CreateEmployee(CreatedEmployeeDto employeeDto)
        {
            unitOfWork.employeeRepositoris.Add(new Employee()
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
            return unitOfWork.Complete();
        }


        public int UpdateEmployee(UpdatedEmployeeDto employeeDto)
        {
            unitOfWork.employeeRepositoris.Update(new Employee()
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
            return unitOfWork.Complete();
        }

        public bool DeleteEmployee(int id)
        {
            var employee = unitOfWork.employeeRepositoris.GetById(id);
            if(employee is not null)
            {
                unitOfWork.employeeRepositoris.Delete(employee);
                return unitOfWork.Complete() > 0;
            }
            return false;
        }


    }
}
