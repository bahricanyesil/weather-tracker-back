using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.Data;
using webapi.Dtos;
using webapi.Models;

namespace webapi.Controllers
{
    [Authorize]
    [Route("api/meetings")]
    [ApiController]
    public class MeetingsController : ControllerBase
    {
        private readonly IMeetingRepo _repository;
        private readonly IMapper _mapper;
        private readonly IContext _context;

        public MeetingsController(IMeetingRepo repository, IMapper mapper, IContext context)
        {
            _repository = repository;
            _mapper = mapper;
            _context = context;
        }

        //GET api/meetings/{id}
        [HttpGet("{id}", Name = "GetMeetingById")]
        public ActionResult<MeetingReadDto> GetMeetingById(string id)
        {
            var meetingItem = _repository.GetMeetingById(id);
            if (meetingItem != null)
            {
                return Ok(new { meeting = _mapper.Map<MeetingReadDto>(meetingItem) });
            }
            return NotFound();
        }

        //POST api/meetings
        [HttpPost]
        public ActionResult<MeetingReadDto> CreateMeeting(MeetingCreateDto meetingCreateDto)
        {
            var meetingModel = _mapper.Map<Meeting>(meetingCreateDto);
            Guid UniqueId = Guid.NewGuid();
            meetingModel.UniqueId = UniqueId.ToString();
            _repository.CreateMeeting(meetingModel, meetingCreateDto.participantIds);
            _repository.SaveChanges();
            var meetingReadDto = _mapper.Map<MeetingReadDto>(meetingModel);
            return CreatedAtRoute(nameof(GetMeetingById), new { id = meetingReadDto.UniqueId }, new { meeting = meetingReadDto });
        }

        //PUT api/meetings/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateMeeting(string id, MeetingUpdateDto meetingUpdateDto)
        {
            var meetingModelFromRepo = _repository.GetMeetingById(id);
            if (meetingModelFromRepo == null)
            {
                return NotFound();
            }
            _mapper.Map(meetingUpdateDto, meetingModelFromRepo);
            _repository.UpdateMeeting(meetingModelFromRepo); // Not doing anything now
            _repository.SaveChanges();
            return NoContent();
        }

        //GET api/meetings/weather
        [HttpGet]
        [Route("weather/{meetingId}")]
        public ActionResult<Weather> GetWeather(string meetingId)
        {
            Meeting meeting = _repository.GetMeetingById(meetingId);
            if (meeting == null)
            {
                return NotFound();
            }
            Weather weather = _repository.GetWeather(meeting.City);
            if (weather == null)
            {
                return NotFound();
            }
            return Ok(new { weather = weather });
        }

    }
}