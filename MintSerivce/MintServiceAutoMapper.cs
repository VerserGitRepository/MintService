using AutoMapper;
using MintSerivce.Models;

namespace MintSerivce
{
    public class MintServiceAutoMapper : Profile
    {
        public MintServiceAutoMapper()
        {
            CreateMap<OrderViewModel, OrderDto>();
            CreateMap<OrderDispatchViewModel, DispatchedOrderDto>();
            CreateMap<CancelledOrdersViewModel, OrderDispatchViewModel>();
        }
    }
}