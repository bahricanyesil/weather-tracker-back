using System.Collections.Generic;
using System.Threading.Tasks;
using webapi.Dtos;
using webapi.Models;

namespace webapi.Data
{
    public interface IFriendRepo
    {
        bool SaveChanges();
        void CreateFriend(Friend friend);
        IEnumerable<Friend> GetFriendsOfUser(string userId);
        IEnumerable<Friend> GetReceivedRequests(string userId);
        IEnumerable<Friend> GetSentRequests(string userId);
        void RejectRequest(Friend friend);
        void AcceptRequest(string senderId, string receiverId);
    }
}