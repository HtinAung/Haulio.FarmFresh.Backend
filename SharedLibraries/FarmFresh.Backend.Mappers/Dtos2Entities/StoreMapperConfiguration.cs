using AutoMapper;
using FarmFresh.Backend.DataTransferObjects;
using FarmFresh.Backend.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmFresh.Backend.Mappers.Dtos2Entities
{
    public class StoreMapperConfiguration:Profile
    {
        public StoreMapperConfiguration()
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

        }
    }
}
