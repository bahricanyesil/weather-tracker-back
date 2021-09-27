using AutoMapper;
using webapi.Dtos;
using webapi.Models;

namespace webapi.Profiles
{
    public class MeetingsProfile : Profile
    {
        public MeetingsProfile()
        {
            CreateMap<Meeting, MeetingReadDto>();
            CreateMap<MeetingCreateDto, Meeting>();
            CreateMap<MeetingUpdateDto, Meeting>();
        }
    }
}