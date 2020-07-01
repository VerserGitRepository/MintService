using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MintSerivce.Models;

namespace MintSerivce
{
    public class MintServiceAutoMapper :Profile
    {
        public MintServiceAutoMapper()
        {
            CreateMap<OrderViewModel, OrderDto>();
            CreateMap<OrderDispatchViewModel, DispatchedOrderDto>();
            CreateMap<CancelledOrdersViewModel, OrderDispatchViewModel>();
        }
    }
}