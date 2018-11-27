using AutoMapper;

namespace RecoverExcelPassword
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Models.Window, Services.Models.Window>()
                .ReverseMap();
        }
    }
}