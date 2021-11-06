using AutoMapper;
using CRUD_API.Dtos;
using CRUD_API.Models;

namespace CRUD_API.MapingProfile
{
    public class ModelMapingProfile : Profile
    {
        public ModelMapingProfile()
        {
            ///<summary>From orginal Databse Model to Dto</summary>
            //Source -> Terget to Get employee
            CreateMap<EmployeeModel, EmployeeModelReadDto>();

            ///<summary>From Dto to Database Model</summary>
            //Terget -> Source To create Employee
            CreateMap<EmployeeModelCreateDto,EmployeeModel>();

            ///<summary>From Dto to Database Model updte the content</summary>
            //Terget -> Source
            CreateMap<EmployeeModelUpdateDto, EmployeeModel>();

            ///<summary>From Dto to Database Model updte the content</summary>
            //Source -> Terget
            CreateMap<EmployeeModel, EmployeeModelUpdateDto>();
        }
    }
}
