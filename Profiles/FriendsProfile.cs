using AutoMapper;
using webapi.Dtos;
using webapi.Models;

namespace webapi.Profiles
{
    public class FriendsProfile : Profile
    {
        public FriendsProfile()
        {
            CreateMap<Meeting, MeetingReadDto>();
        }
    }
}