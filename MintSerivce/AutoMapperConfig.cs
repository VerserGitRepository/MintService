using AutoMapper;

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