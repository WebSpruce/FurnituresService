using AutoMapper;
using FurnituresServiceApi.Dto;
using FurnituresServiceModels.Models;
using Microsoft.AspNetCore.Identity;

namespace FurnituresServiceApi.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Furniture, FurnitureDto>();
            CreateMap<FurnitureDto, Furniture>();
            CreateMap<IdentityUser, UserDto>();
            CreateMap<UserDto, IdentityUser>();
            CreateMap<Cart, CartDto>();
            CreateMap<CartDto, Cart>();
            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();
            CreateMap<Order,OrderDto>();
            CreateMap<OrderDto, Order>();
        }
    }
}
