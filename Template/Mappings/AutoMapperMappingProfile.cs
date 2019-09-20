using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template.Core.DTO;
using Template.Core.Entity;

namespace Template.Mappings
{
    public class AutoMapperMappingProfile : Profile
    {
        public AutoMapperMappingProfile()
        {
            this.CreateMap<LoginDTO, ApplicationUser>();
            this.CreateMap<AuthStore, LoginDTO>();

        }
    }
}
