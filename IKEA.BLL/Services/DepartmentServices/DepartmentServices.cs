using IKEA.BLL.Dto_s.Departments;
using IKEA.DAL.Models.Departments;
using IKEA.DAL.Persistance.Repositories.Departments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.BLL.Services.DepartmentServices
{
    public class DepartmentServices:IDepartmentServices 
    {
        private IDepartmentRepository Repository;
        public DepartmentServices(IDepartmentRepository repository) => Repository = repository;

        public IEnumerable<DepartmentDto> GetAllDepartments()
        => Repository.GetAll().Where(D => !D.IsDeleted).Select(D => new DepartmentDto
        {
            Id = D.Id,
            Name = D.Name,
            Code = D.Code,
            CreationDate = D.CreationDate
        });

        public DepartmentDetailsDto? GetDepartmentById(int id)
        {
            var Deparment = Repository.GetById(id);
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
        public int CreateDepartment(CreatedDepartmenDto departmentDto) => Repository.Add(new Department()
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

        public int UpdateDepartment(UpdatedDepartmentDto departmentDto) => Repository.Update(new Department()
        {
            Id = departmentDto.Id,
            Code = departmentDto.Code,
            Name = departmentDto.Name,
            Description = departmentDto.Description,
            CreationDate = departmentDto.CreationDate,
            LastModifiedBy = 1,
            LastModifiedOn = DateTime.Now
        });

        public bool DeleteDepartment(int id)
            => Repository.GetById(id) is not null ? Repository.Delete(Repository.GetById(id)) > 0:false;

    }
}
