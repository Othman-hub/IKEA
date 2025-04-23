using AutoMapper;
using IKEA.BLL.Dto_s.Departments;
using IKEA.PL.ViewModels;

namespace IKEA.PL.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<DepartmentVM, CreatedDepartmenDto>().ReverseMap();
            CreateMap<DepartmentDetailsDto, DepartmentVM>().ReverseMap();
            CreateMap<UpdatedDepartmentDto, DepartmentVM>().ReverseMap();
        }
    }
}
