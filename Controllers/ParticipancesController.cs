using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.Data;
using webapi.Dtos;
using webapi.Models;

namespace webapi.Controllers
{
    [Authorize]
    [Route("api/participances")]
    [ApiController]
    public class ParticipancesController : ControllerBase
    {
        private readonly IParticipanceRepo _repository;
        private readonly IMapper _mapper;
        private readonly IContext _context;

        public ParticipancesController(IParticipanceRepo repository, IMapper mapper, IContext context)
        {
            _repository = repository;
            _mapper = mapper;
            _context = context;
        }

        //GET api/participances/user/{id}
        [HttpGet]
        [Route("user/{id}")]
        public ActionResult<IEnumerable<MeetingReadDto>> GetParticipancesByUserId(string id)
        {
            var participanceItems = _repository.GetParticipancesByUserId(id);
            if (participanceItems.Count() == 0)
            {
                return NotFound();
            }
            List<MeetingReadDto> meetings = new List<MeetingReadDto>();
            foreach (Participance item in participanceItems)
            {
                Meeting meeting = _context.Meetings.FirstOrDefault(localMeeting => localMeeting.UniqueId == item.MeetingId);
                if (meeting != null)
                {
                    meetings.Add(_mapper.Map<MeetingReadDto>(meeting));
                }
            }
            return Ok(new { meetings = meetings });
        }

        //GET api/participances/meeting/{id}
        [HttpGet]
        [Route("meeting/{id}")]
        public ActionResult<IEnumerable<UserReadDto>> GetParticipancesByMeetingId(string id)
        {
            var participanceItems = _repository.GetParticipancesByMeetingId(id);
            List<UserReadDto> users = new List<UserReadDto>();
            foreach (Participance item in participanceItems)
            {
                User user = _context.Users.FirstOrDefault(localUser => localUser.UniqueId == item.UserId);
                if (user != null)
                {
                    users.Add(_mapper.Map<UserReadDto>(user));
                }
            }
            return Ok(new { users = users });
        }

        [HttpGet("{id}", Name = "GetParticipanceById")]
        public ActionResult<Participance> GetParticipanceById(int id)
        {
            var participanceItem = _repository.GetParticipanceById(id);
            if (participanceItem != null)
            {
                return Ok(new { participance = participanceItem });
            }
            return NotFound();
        }

        [HttpDelete]
        public ActionResult DeleteParticipance(ParticipanceDeleteDto participanceDeleteDto)
        {
            var participanceFromRepo = _context.Participances.FirstOrDefault(p => p.MeetingId == participanceDeleteDto.MeetingId && p.UserId == participanceDeleteDto.UserId);
            if (participanceFromRepo == null)
            {
                return NotFound();
            }
            _repository.DeleteParticipance(participanceFromRepo);
            _repository.SaveChanges();
            return Ok();
        }
    }
}