using AutoMapper;
using WebApi.Dto;
using WebApi.Models;

namespace WebApi.Repo
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>(MemberList.Destination).ReverseMap();

            CreateMap<ProductGroup, ProductGroupDto>(MemberList.Destination).ReverseMap();

            CreateMap<Store, StoreDto>(MemberList.Destination).ReverseMap();
        }
        
    }
}
