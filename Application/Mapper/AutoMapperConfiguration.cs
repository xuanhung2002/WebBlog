using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBlog.Application.Dtos;
using WebBlog.Application.Dtos.ApiRequestDtos;

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
