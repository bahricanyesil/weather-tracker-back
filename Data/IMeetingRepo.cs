using System.Collections.Generic;
using System.Threading.Tasks;
using webapi.Models;

namespace webapi.Data
{
    public interface IMeetingRepo
    {
        bool SaveChanges();
        Meeting GetMeetingById(string id);
        void CreateMeeting(Meeting meeting, IEnumerable<string> participantIds);
        void UpdateMeeting(Meeting meeting);
        void DeleteMeeting(Meeting meeting);
        Weather GetWeather(string city);
    }
}