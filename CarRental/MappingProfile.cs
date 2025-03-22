using AutoMapper;
using CarRental.Models.DTOs.Role;
using CarRental.Models.DTOs.User;
using CarRental.Models.Entities;

namespace CarRental
{
    public class MappinProfile : Profile
    {
        public MappinProfile()
        {
            //CreateMap<Car, GetCarDTO>().ReverseMap();
            CreateMap<User, UserGetDTO>().ReverseMap();

            CreateMap<Role, GetRoleDTO>().ReverseMap();
        }
    }
}
