using System.Collections.Generic;
using webapi.Models;

namespace webapi.Data
{
    public interface IParticipanceRepo
    {
        bool SaveChanges();
        IEnumerable<Participance> GetParticipancesByUserId(string userId);
        IEnumerable<Participance> GetParticipancesByMeetingId(string meetingId);
        void DeleteParticipance(Participance participance);
        Participance GetParticipanceById(int id);
    }
}