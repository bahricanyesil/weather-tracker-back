using System.Collections.Generic;
using System.Threading.Tasks;
using webapi.Models;

namespace webapi.Data
{
    public interface IUserRepo
    {
        bool SaveChanges();
        IEnumerable<User> GetAllUsers();
        User GetUserById(string id);
        void CreateUser(User user);
        void UpdateUser(User user);

        IEnumerable<User> GetUsersByName(string username);
        User GetUserByEmail(string email);
        string ValidateUser(string email, string password);
        void ChangePassword(string userId, string newPassword);

        Task sendEmail(string email, string name, string emailCode);
        void VerifyUser(string email);
    }
}