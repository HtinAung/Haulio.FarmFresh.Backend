using AutoMapper;
using FarmFresh.Backend.DataTransferObjects;
using FarmFresh.Backend.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FarmFresh.Backend.Mappers.Dtos2Entities
{
    public class Dto2EntitiesMapperConfiguration:Profile
    {
        public Dto2EntitiesMapperConfiguration()
        {
            Init();
        }

        public void Init()
        {
            CreateMap<AppProduct, ProductDto>()
                .ForMember(dest => dest.CategoryName, src => src.MapFrom(prop => prop.Category != null ? prop.Category.Name ?? "-" : "-"))
                .ForMember(dest => dest.StoreName, src => src.MapFrom(prop => prop.Store != null ? prop.Store.Name ?? "-" : "-"));

            CreateMap<ProductDto, AppProduct>();

            CreateMap<AppProductCategory, ProductCategoryDto>().ReverseMap();

            CreateMap<AppOrderHistory, OrderHistoryDto>()
                .ForMember(dest => dest.StoreName, src => src.MapFrom(prop => prop.Store != null ? prop.Store.Name ?? "-" : "-"))
                .ForMember(dest => dest.UserName, src => src.MapFrom(prop => prop.User != null ? prop.User.FullName ?? "-" : "-"));

            CreateMap<OrderHistoryDto, AppOrderHistory>();

            CreateMap<AppStore, StoreDto>()
                .ForMember(dest => dest.AdminUserName, src => src.MapFrom(prop => prop.User != null ? prop.User.FullName ?? "-" : "-"))
                .ForMember(dest => dest.AdminUserId, src => src.MapFrom(prop => prop.UserId));

            CreateMap<StoreDto, AppStore>()
                .ForMember(dest => dest.UserId, src => src.MapFrom(prop => prop.AdminUserId));

            CreateMap<AppUser, UserDto>()
                .ForMember(dest => dest.AddressLine,
                    src => src.MapFrom(prop => prop.Addresses.FirstOrDefault() != null ? prop.Addresses.FirstOrDefault().AddressLine ?? "-" : "-"))
                .ForMember(dest => dest.City,
                    src => src.MapFrom(prop => prop.Addresses.FirstOrDefault() != null ? prop.Addresses.FirstOrDefault().City ?? "-" : "-"))
                .ForMember(dest => dest.Region,
                    src => src.MapFrom(prop => prop.Addresses.FirstOrDefault() != null ? prop.Addresses.FirstOrDefault().Region ?? "-" : "-"))
                .ForMember(dest => dest.Country,
                    src => src.MapFrom(prop => prop.Addresses.FirstOrDefault() != null ? prop.Addresses.FirstOrDefault().Country ?? "-" : "-"))
                .ForMember(dest => dest.PostalCode,
                    src => src.MapFrom(prop => prop.Addresses.FirstOrDefault() != null ? prop.Addresses.FirstOrDefault().PostalCode ?? "-" : "-"))
                .ForMember(dest => dest.StoreName,
                    src => src.MapFrom(prop => prop.Store != null ? prop.Store.Name ?? "-" : "-"));

        }
    }
}
