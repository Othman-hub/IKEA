using IKEA.BLL.Common.Services.Attachments;
using IKEA.BLL.Dto_s.Employees;
using IKEA.DAL.Models.Employees;
using IKEA.DAL.Persistance.Repositories.Employees;
using IKEA.DAL.Persistance.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace IKEA.BLL.Services.EmployeeServices
{
    public class EmployeeServices:IEmployeeServices
    {
        
        private readonly IUnitOfWork unitOfWork;
        private readonly IAttachmentServices attachmentServices;

        public EmployeeServices(IUnitOfWork unitOfWork,IAttachmentServices attachmentServices)
        {;
            this.unitOfWork = unitOfWork;
            this.attachmentServices = attachmentServices;
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllEmployees(string search) =>
            await unitOfWork.employeeRepositoris.GetAll().Where(E => !E.IsDeleted &&  (string.IsNullOrEmpty(search) || E.Name.ToLower().Contains(search.ToLower()))).Include(E => E.Department).Select(E => new EmployeeDto()
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
            }).ToListAsync();


        public async Task<EmployeeDetailsDto?> GetEmployeeById(int id)
        {
            var E = await unitOfWork.employeeRepositoris.GetById(id);
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
                       Department = E.Department?.Name ?? "N/A",
                       ImageName = E.ImageName
                };
            return null;
        }

        public async Task<int> CreateEmployee(CreatedEmployeeDto employeeDto)
        {
            var Employee = new Employee()
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

            };
            if(employeeDto.Image is not null)
            {
                Employee.ImageName = attachmentServices.UplodImage(employeeDto.Image, "images");
            }
            unitOfWork.employeeRepositoris.Add(Employee);
            return await unitOfWork.Complete();
        }


        public async Task<int> UpdateEmployee(UpdatedEmployeeDto employeeDto)
        {
            var Employee = new Employee()
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
                ImageName = employeeDto.ImageName,
            };
            if(employeeDto.Image is not null)
            {
                if(Employee.ImageName is not null)
                {
                    attachmentServices.DeleteImage(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", "images", Employee.ImageName));
                }
                Employee.ImageName = attachmentServices.UplodImage(employeeDto.Image, "images");
            }
            unitOfWork.employeeRepositoris.Update(Employee);
            return await unitOfWork.Complete();
        }

        public async Task<bool> DeleteEmployee(int id)
        {
            var employee = await unitOfWork.employeeRepositoris.GetById(id);

            if(employee is not null)
            {
                if(employee.ImageName is not null)
                    attachmentServices.DeleteImage(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", "images",employee.ImageName));

                unitOfWork.employeeRepositoris.Delete(employee);
                return await unitOfWork.Complete() > 0;
            }
            return false;
        }


    }
}
