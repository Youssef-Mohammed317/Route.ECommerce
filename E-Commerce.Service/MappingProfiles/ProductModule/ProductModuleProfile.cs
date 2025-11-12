using AutoMapper;
using E_Commerce.Domian.Entites.ProductModule;
using E_Commerce.Shared.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Service.Implementation.MappingProfiles.ProductModule
{
    public class ProductModuleProfile : Profile
    {
        public ProductModuleProfile()
        {
            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.ProductBrand, opt => opt.MapFrom(src => src.ProductBrand.Name))
                .ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => src.ProductType.Name))
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom<ProductPictureUrlResolver>())
                .ReverseMap();

            CreateMap<ProductBrand, ProductBrandDTO>().ReverseMap();

            CreateMap<ProductType, ProductTypeDTO>().ReverseMap();
        }
    }
}
