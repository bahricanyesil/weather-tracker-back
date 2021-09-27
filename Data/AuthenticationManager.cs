using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace webapi.Data
{
    public class JWTAuthenticationManager : IAuthenticationManager
    {

        private readonly string _tokenKey;
        private readonly IContext _context;
        public JWTAuthenticationManager(string tokenKey, IContext context)
        {
            _tokenKey = tokenKey;
            _context = context;
        }

        public string Authenticate(string email, string password)
        {
            if (!_context.Users.Any(u => u.Email == email && u.Password == password))
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_tokenKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, email)
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