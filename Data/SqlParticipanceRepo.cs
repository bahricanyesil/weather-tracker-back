using System.Collections.Generic;
using System.Linq;
using webapi.Models;

namespace webapi.Data
{
    public class SqlParticipanceRepo : IParticipanceRepo
    {
        private readonly IContext _context;

        public SqlParticipanceRepo(IContext context)
        {
            _context = context;
        }
        public void DeleteParticipance(Participance participance)
        {
            _context.Participances.Remove(participance);
        }

        public Participance GetParticipanceById(int id)
        {
            return _context.Participances.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Participance> GetParticipancesByMeetingId(string meetingId)
        {
            return _context.Participances.Where(p => p.MeetingId == meetingId).ToList();
        }

        public IEnumerable<Participance> GetParticipancesByUserId(string userId)
        {
            return _context.Participances.Where(p => p.UserId == userId).ToList();
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}