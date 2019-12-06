using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MintSerivce
{
    public class AutoMapperConfig : Profile
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg => cfg.AddProfile<MintServiceAutoMapper>());            
        }
    }
}