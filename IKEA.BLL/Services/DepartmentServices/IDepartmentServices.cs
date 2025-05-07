using IKEA.BLL.Dto_s.Departments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.BLL.Services.DepartmentServices
{
    public interface IDepartmentServices
    {
        Task<IEnumerable<DepartmentDto>> GetAllDepartments();
        Task<DepartmentDetailsDto>? GetDepartmentById(int id);
        Task<int> CreateDepartment(CreatedDepartmenDto departmenDto);
        Task<int> UpdateDepartment(UpdatedDepartmentDto departmentDto);
        Task<bool> DeleteDepartment(int id);

    }
}
