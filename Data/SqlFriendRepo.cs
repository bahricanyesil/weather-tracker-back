using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using webapi.Dtos;
using webapi.Models;

namespace webapi.Data
{
    public class SqlFriendRepo : IFriendRepo
    {
        private readonly IContext _context;

        public SqlFriendRepo(IContext context)
        {
            _context = context;
        }

         public void CreateFriend(Friend friend)
        {
            if (friend == null)
            {
                throw new ArgumentNullException(nameof(friend));
            }
            _context.Friends.Add(friend);
        }

        public void AcceptRequest(string senderId, string receiverId) {
            var friend = new Friend();
            friend = (_context.Friends.Where(a => a.SenderId == senderId && a.ReceiverId == receiverId)).SingleOrDefault();
            if (friend != null)
            {
                friend.isWaiting = false;
                _context.Entry(friend).State = EntityState.Modified;
            }
        }

        public IEnumerable<Friend> GetFriendsOfUser(string userId)
        {
            return _context.Friends.Where(p => (p.ReceiverId == userId || p.SenderId == userId) && !p.isWaiting).ToList();
        }
        public IEnumerable<Friend> GetReceivedRequests(string userId)
        {
            return _context.Friends.Where(p => p.ReceiverId == userId && p.isWaiting).ToList();
        }

        public IEnumerable<Friend> GetSentRequests(string userId)
        {
            return _context.Friends.Where(p => p.SenderId == userId && p.isWaiting).ToList();
        }

        public void RejectRequest(Friend friend)
        {
            _context.Remove(friend);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}