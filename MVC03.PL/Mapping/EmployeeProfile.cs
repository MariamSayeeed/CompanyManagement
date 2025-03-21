using AutoMapper;
using MVC03.DAL.Models;
using MVC03.PL.Dtos;

namespace MVC03.PL.Mapping
{
    public class EmployeeProfile : Profile
    {

        public EmployeeProfile() 
        {
            CreateMap<CreateEmployeeDto, Employee>();
            CreateMap<Employee , CreateEmployeeDto>();
        }
    }
}
