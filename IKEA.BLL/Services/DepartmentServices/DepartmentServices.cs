using IKEA.BLL.Dto_s.Departments;
using IKEA.DAL.Models.Departments;
using IKEA.DAL.Persistance.Repositories.Departments;
using IKEA.DAL.Persistance.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.BLL.Services.DepartmentServices
{
    public class DepartmentServices:IDepartmentServices 
    {
       
        private readonly IUnitOfWork unitOfWork;

        public DepartmentServices(IUnitOfWork unitOfWork)
        {
            
            this.unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<DepartmentDto>> GetAllDepartments()
        => await unitOfWork.departmentRepository.GetAll().Where(D => !D.IsDeleted).Select(D => new DepartmentDto
        {
            Id = D.Id,
            Name = D.Name,
            Code = D.Code,
            CreationDate = D.CreationDate
        }).ToListAsync();

        public async Task<DepartmentDetailsDto?> GetDepartmentById(int id)
        {
            var Deparment = await unitOfWork.departmentRepository.GetById(id);
            if(Deparment is not null)
            {
                return new DepartmentDetailsDto
                {
                    Id = Deparment.Id,
                    Name = Deparment.Name,
                    Code = Deparment.Code,
                    CreationDate = Deparment.CreationDate,
                    LastModifiedBy = Deparment.LastModifiedBy,
                    IsDeleted = Deparment.IsDeleted,
                    LastModifiedOn = Deparment.LastModifiedOn,
                    CreatedBy = Deparment.CreatedBy,
                    CreatedOn = Deparment.CreatedOn,
                    Description = Deparment.Description
                };
            }
            return null;
        }
        public async Task<int> CreateDepartment(CreatedDepartmenDto departmentDto)
        {
            unitOfWork.departmentRepository.Add(new Department()
            {
                Code = departmentDto.Code,
                Name = departmentDto.Name,
                Description = departmentDto.Description,
                CreationDate = departmentDto.CreationDate,
                CreatedBy = 1,
                CreatedOn = DateTime.Now,
                LastModifiedBy = 1,
                LastModifiedOn = DateTime.Now
            });
            return await unitOfWork.Complete();
        }

        public async Task<int> UpdateDepartment(UpdatedDepartmentDto departmentDto)
        {
            unitOfWork.departmentRepository.Update(new Department()
            {
                Id = departmentDto.Id,
                Code = departmentDto.Code,
                Name = departmentDto.Name,
                Description = departmentDto.Description,
                CreationDate = departmentDto.CreationDate,
                LastModifiedBy = 1,
                LastModifiedOn = DateTime.Now
            });
            return await unitOfWork.Complete();
        }

        public async Task<bool> DeleteDepartment(int id)
        {
            var department = await unitOfWork.departmentRepository.GetById(id);
            if (department is not null)
            {
                unitOfWork.departmentRepository.Delete(department);
                return await unitOfWork.Complete() > 0;
            }
            return false;
        }

    }
}
