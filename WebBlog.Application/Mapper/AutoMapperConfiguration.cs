using AutoMapper;
using WebBlog.Application.Dto;

namespace WebBlog.Application.Mapper
{
    public class AutoMapperConfiguration
    {
        public static MapperConfiguration RegisterMap()
        {
            var mappingConfig = new MapperConfiguration(config =>
                {
                    config.CreateMap<CreateUserRequest, UserDto>();
                }  
            );

            return mappingConfig;
        }
    }
}
