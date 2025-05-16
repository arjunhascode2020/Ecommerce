using AutoMapper;
using Basket.Application.Responses;
using Basket.Core.Entities;

namespace Basket.Application.Mappers
{
    class BasketMappingProfile:Profile
    {
        public BasketMappingProfile()
        {
            CreateMap<ShoppingCartItem, ShoppingCartItemResponse>()
                .ReverseMap();
            CreateMap<ShoppingCart, ShoppingCartResponse>().ReverseMap();
        }
    }
}
