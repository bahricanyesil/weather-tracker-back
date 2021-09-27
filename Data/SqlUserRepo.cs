using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using FluentEmail.Core;
using FluentEmail.Smtp;
using Microsoft.EntityFrameworkCore;
using webapi.Models;

namespace webapi.Data
{
    public class SqlUserRepo : IUserRepo
    {
        private readonly IContext _context;

        public SqlUserRepo(IContext context)
        {
            _context = context;
        }

        public void CreateUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            _context.Users.Add(user);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users.Where(u => u.IsVerified).ToList();
        }

        public User GetUserById(string id)
        {
            return _context.Users.FirstOrDefault(p => p.UniqueId == id && p.IsVerified);
        }

        public User GetUserByEmail(string email)
        {
            return _context.Users.FirstOrDefault(p => p.Email == email);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void UpdateUser(User user)
        {
            // Not doing anything now
        }

        public IEnumerable<User> GetUsersByName(string username)
        {
            return _context.Users.Where(user => user.UserName.ToLower().Contains(username.ToLower()) && user.IsVerified).ToList();
        }

        public string ValidateUser(string email, string password)
        {
            User user = _context.Users.FirstOrDefault((u) => u.Email == email && u.Password == password && u.IsVerified);
            if (user == null)
            {
                return null;
            }
            return user.UniqueId;
        }

        public void ChangePassword(string userId, string newPassword)
        {
            var user = new User();
            user = (_context.Users.Where(a => a.UniqueId == userId)).SingleOrDefault();
            if (user != null)
            {
                user.Password = newPassword;
                _context.Entry(user).State = EntityState.Modified;
            }

        }

        public void VerifyUser(string email)
        {
            var user = new User();
            user = (_context.Users.Where(a => a.Email == email)).SingleOrDefault();
            if (user != null)
            {
                user.IsVerified = true;
                _context.Entry(user).State = EntityState.Modified;
            }
        }

        public async Task sendEmail(string userEmail, string name, string emailCode)
        {
            var sender = new SmtpSender(() => new SmtpClient(host: "localhost")
            {
                EnableSsl = false,
                DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
                PickupDirectoryLocation = @"C:\Demos"
            });

            Email.DefaultSender = sender;

            var email = await Email
                .From(emailAddress: "weathertracker@belbim.com")
                .To(emailAddress: userEmail, name: name)
                .Subject($"Welcome {name}!")
                .Body(body: $"Thank you to register, here is your validation code: {emailCode}!")
                .SendAsync();
        }
    }
}