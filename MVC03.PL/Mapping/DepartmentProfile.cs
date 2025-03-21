using AutoMapper;
using MVC03.DAL.Models;
using MVC03.PL.Dtos;

namespace MVC03.PL.Mapping
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile() 
        {
            CreateMap<CreateDepartmentDto, Department>();
            CreateMap<Department,CreateDepartmentDto >();
            //CreateMap<Department,CreateDepartmentDto >()
            //            .ForMember(D=> D.Name, X=> X.MapFrom(s=>s.Name) );
        }
    }
}
