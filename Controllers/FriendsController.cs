using System;
using System.Collections.Generic;
using System.Linq;
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
    [Route("api/friends")]
    [ApiController]
    public class FriendsController : ControllerBase
    {
        private readonly IFriendRepo _repository;
        private readonly IMapper _mapper;
        private readonly IContext _context;

        public FriendsController(IFriendRepo repository, IMapper mapper, IContext context)
        {
            _repository = repository;
            _mapper = mapper;
            _context = context;
        }

        //GET api/friends/{userId}
        [HttpGet("{userId}", Name = "GetFriendsOfUser")]
        public ActionResult<IEnumerable<UserReadDto>> GetFriendsOfUser(string userId)
        {
            var friends = _repository.GetFriendsOfUser(userId);
            List<UserReadDto> users = GetUsersForFriendsOrRequests(friends, "friends", userId);
            return Ok(new { friends = users });
        }

        //GET api/friends/receivedRequests/{userId}
        [HttpGet(Name = "getReceviedRequests")]
        [Route("receivedRequests/{userId}")]
        public ActionResult<IEnumerable<UserReadDto>> GetReceviedRequests(string userId)
        {
            var friends = _repository.GetReceivedRequests(userId);
            List<UserReadDto> users = GetUsersForFriendsOrRequests(friends, "requests", userId);
            return Ok(new { users = users });
        }

        private List<UserReadDto> GetUsersForFriendsOrRequests(IEnumerable<Friend> friends, string type, string userId)
        {
            List<UserReadDto> users = new List<UserReadDto>();
            foreach (Friend item in friends)
            {
                if (type == "friends")
                {
                    string searchId = item.SenderId;
                    if (searchId == userId) searchId = item.ReceiverId;
                    User user = _context.Users.FirstOrDefault(localUser => localUser.UniqueId == searchId && localUser.IsVerified);
                    if (user != null)
                    {
                        users.Add(_mapper.Map<UserReadDto>(user));
                    }
                }
                else if (type == "sends")
                {
                    User user = _context.Users.FirstOrDefault(localUser => (localUser.UniqueId == item.ReceiverId && localUser.IsVerified));
                    if (user != null)
                    {
                        users.Add(_mapper.Map<UserReadDto>(user));
                    }
                }
                else
                {
                    User user = _context.Users.FirstOrDefault(localUser => (localUser.UniqueId == item.SenderId && localUser.IsVerified));
                    if (user != null)
                    {
                        users.Add(_mapper.Map<UserReadDto>(user));
                    }
                }
            }
            return users;
        }

        //GET api/friends/requestAndFriends/{userId}
        [HttpGet]
        [Route("requestAndFriends/{userId}")]
        public ActionResult<IEnumerable<UserReadDto>> GetRequestsAndFriends(string userId)
        {
            var localRequests = _repository.GetReceivedRequests(userId);
            var localFriends = _repository.GetFriendsOfUser(userId);
            var localSends = _repository.GetSentRequests(userId);
            IEnumerable<UserReadDto> receivedRequests = GetUsersForFriendsOrRequests(localRequests, "requests", userId);
            IEnumerable<UserReadDto> friends = GetUsersForFriendsOrRequests(localFriends, "friends", userId);
            IEnumerable<UserReadDto> sends = GetUsersForFriendsOrRequests(localSends, "sends", userId);
            return Ok(new { requests = receivedRequests, friends = friends, sends = sends });
        }

        //PUT api/friends/rejectRequest
        [HttpPut]
        [Route("reject")]
        public ActionResult RejectRequest(FriendUpdateDto friendUpdateDto)
        {
            Friend friend = _context.Friends.FirstOrDefault(localFriend => localFriend.SenderId == friendUpdateDto.SenderId && localFriend.ReceiverId == friendUpdateDto.ReceiverId);
            if (friend == null)
            {
                return NotFound();
            }
            _repository.RejectRequest(friend);
            _repository.SaveChanges();
            return Ok();
        }

        //PUT api/friends/accept
        [HttpPut]
        [Route("accept")]
        public ActionResult AcceptRequest(FriendUpdateDto friendUpdateDto)
        {
            Friend friendFromRepo = _context.Friends.FirstOrDefault(localFriend => localFriend.SenderId == friendUpdateDto.SenderId && localFriend.ReceiverId == friendUpdateDto.ReceiverId);
            if (friendFromRepo == null)
            {
                return NotFound();
            }
            _repository.AcceptRequest(friendUpdateDto.SenderId, friendUpdateDto.ReceiverId);
            _repository.SaveChanges();
            return Ok(new { responseMessage = "Success" });
        }

        //Post api/friends
        [HttpPost]
        public ActionResult CreateFriend(FriendCreateDto friendCreateDto)
        {
            var friend = _context.Friends.FirstOrDefault(localFriend => localFriend.SenderId == friendCreateDto.SenderId && localFriend.ReceiverId == friendCreateDto.ReceiverId);
            if (friend != null)
            {
                return BadRequest("There is already a friendship between the given users.");
            }
            var friendModel = _mapper.Map<Friend>(friendCreateDto);
            Guid UniqueId = Guid.NewGuid();
            friendModel.UniqueId = UniqueId.ToString();
            _repository.CreateFriend(friendModel);
            _repository.SaveChanges();
            return Ok();
        }
    }
}