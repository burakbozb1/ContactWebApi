using AutoMapper;
using ContactWeb.Core.DTOs;
using ContactWeb.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactWeb.Service.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<User, UserLoginDto>().ReverseMap();
            CreateMap<User, UserLoginResponseDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, UserSignUpDto>().ReverseMap();
            CreateMap<Contact, AddContactDto>().ReverseMap();
            CreateMap<Contact, ContactDto>().ReverseMap();
            CreateMap<Location, LocationDto>().ReverseMap();
            CreateMap<SearchLocationDto, LocationDto>().ReverseMap();
            CreateMap<OldContact, OldContactDto>().ReverseMap();
            CreateMap<Contact, ContactWithOldContactsDto>().ReverseMap();
        }
    }
}
