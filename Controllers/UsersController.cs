using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using webapi.Data;
using webapi.Dtos;
using webapi.Models;

namespace webapi.Controllers
{
    [Authorize]
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepo _repository;
        private readonly IMapper _mapper;
        private IConfiguration _config;

        public UsersController(IUserRepo repository, IMapper mapper, IConfiguration config)
        {
            _repository = repository;
            _mapper = mapper;
            _config = config;
        }

        //GET api/users
        [HttpGet]
        public ActionResult<IEnumerable<UserReadDto>> GetAllUsers()
        {
            var userItems = _repository.GetAllUsers();
            return Ok(_mapper.Map<IEnumerable<UserReadDto>>(userItems));
        }

        //GET api/users
        [HttpGet]
        [Route("name/{username}")]
        public ActionResult<IEnumerable<UserReadDto>> GetUsersByName(string username)
        {
            var userItems = _repository.GetUsersByName(username);
            return Ok(new { users = _mapper.Map<IEnumerable<UserReadDto>>(userItems) });
        }

        //GET api/users/{id}
        [HttpGet("{id}", Name = "GetUserById")]
        public ActionResult<UserReadDto> GetUserById(string id)
        {
            var userItem = _repository.GetUserById(id);
            if (userItem != null)
            {
                return Ok(new { user = _mapper.Map<UserReadDto>(userItem)});
            }
            return NotFound();
        }

        //POST api/users
        [AllowAnonymous]
        [HttpPost]
        public ActionResult<UserReadDto> CreateUser(UserCreateDto userCreateDto)
        {
            var user = _repository.GetUserByEmail(userCreateDto.Email);
            if (user != null)
            {
                return BadRequest("There is already a user with the given email.");
            }
            var userModel = _mapper.Map<User>(userCreateDto);
            Guid UniqueId = Guid.NewGuid();
            userModel.UniqueId = UniqueId.ToString();
            _repository.CreateUser(userModel);
            Random rnd = new Random();
            string emailCode = rnd.Next(1000, 9999).ToString();
            string emailToken = CreateToken(emailCode, "number");
            if (emailToken == null)
            {
                return NotFound();
            }
            _repository.sendEmail(userCreateDto.Email, userCreateDto.UserName, emailCode);
            _repository.SaveChanges();
            var userReadDto = _mapper.Map<UserReadDto>(userModel);
            return CreatedAtRoute(nameof(GetUserById), new { id = userReadDto.UniqueId }, new { user = userReadDto, token = emailToken });
        }

        //POST api/users/forgotPass
        [AllowAnonymous]
        [HttpGet]
        [Route("forgotPass/{email}")]
        public ActionResult ForgotPassword(string email)
        {
            User user = _repository.GetUserByEmail(email);
            if (user == null)
            {
                return BadRequest("There is no user with the given email.");
            }
            Random rnd = new Random();
            string emailCode = rnd.Next(1000, 9999).ToString();
            string emailToken = CreateToken(emailCode, "number");
            if (emailToken == null)
            {
                return NotFound();
            }
            _repository.sendEmail(email, user.UserName, emailCode);
            _repository.SaveChanges();
            return Ok(new { token = emailToken });
        }

        //PUT api/users/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateUser(string id, UserUpdateDto userUpdateDto)
        {
            var userModelFromRepo = _repository.GetUserById(id);
            if (userModelFromRepo == null)
            {
                return NotFound();
            }
            _mapper.Map(userUpdateDto, userModelFromRepo);
            _repository.UpdateUser(userModelFromRepo); // Not doing anything now
            _repository.SaveChanges();
            return NoContent();
        }

        //PATCH api/users/{id}
        [HttpPatch("{id}")]
        public ActionResult PartialUserUpdate(string id, JsonPatchDocument<UserUpdateDto> patchDoc)
        {
            var userModelFromRepo = _repository.GetUserById(id);
            if (userModelFromRepo == null)
            {
                return NotFound();
            }

            var userToPatch = _mapper.Map<UserUpdateDto>(userModelFromRepo);
            patchDoc.ApplyTo(userToPatch, ModelState);

            if (!TryValidateModel(userToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(userToPatch, userModelFromRepo);
            _repository.UpdateUser(userModelFromRepo);
            _repository.SaveChanges();
            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public ActionResult Login(UserLoginDto login)
        {
            ActionResult response = Unauthorized();
            var tokenString = Authenticate(login.Email, login.Password);
            if (tokenString != null)
            {
                User userModel = _repository.GetUserByEmail(login.Email);
                var userReadDto = _mapper.Map<UserReadDto>(userModel);
                response = Ok(new { user = userReadDto, token = tokenString });
            }

            return response;
        }

        [HttpPost]
        [Route("changePass")]
        public ActionResult ChangePassword(UserChangePassDto changePassDto)
        {
            string userId = _repository.ValidateUser(changePassDto.Email, changePassDto.OldPassword);
            if (userId == null)
            {
                return NotFound();
            }
            _repository.ChangePassword(userId, changePassDto.NewPassword);
            _repository.SaveChanges();
            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("verify")]
        public ActionResult VerifyEmail(UserVerifyEmailDto verifyContent)
        {
            var user = _repository.GetUserByEmail(verifyContent.Email);
            if (user == null)
            {
                return NotFound();
            }
            if (user.IsVerified)
            {
                return BadRequest("You have already verified your account.");
            }
            var stream = verifyContent.Token;
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;
            if (verifyContent.Code != tokenS.Claims.First(claim => claim.Type == "number").Value)
            {
                return ValidationProblem("You entered wrong code, check again!");
            }
            _repository.VerifyUser(verifyContent.Email);
            string tokenInString = CreateToken(verifyContent.Email);
            _repository.SaveChanges();
            return Ok(new { token = tokenInString, userId = user.UniqueId });
        }

        private string Authenticate(string email, string password)
        {
            if (_repository.ValidateUser(email, password) == null)
            {
                return null;
            }

            return CreateToken(email);
        }

        private string CreateToken(string tokenInfo, string type = "email")
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = _config.GetValue<string>("JwtKey");
            var key = Encoding.ASCII.GetBytes(tokenKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(type == "email" ? ClaimTypes.Email : "number", tokenInfo)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}