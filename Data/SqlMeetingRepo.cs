using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RestSharp;
using webapi.Models;

namespace webapi.Data
{
    public class SqlMeetingRepo : IMeetingRepo
    {
        private readonly IContext _context;

        public SqlMeetingRepo(IContext context)
        {
            _context = context;
        }

        public void CreateMeeting(Meeting Meeting, IEnumerable<string> participantIds)
        {
            if (Meeting == null)
            {
                throw new ArgumentNullException(nameof(Meeting));
            }
            _context.Meetings.Add(Meeting);
            foreach (var userId in participantIds)
            {
                _context.Participances.Add(new Participance { MeetingId = Meeting.UniqueId, UserId = userId });
            }
        }

        public void UpdateMeeting(Meeting Meeting)
        {
            // Not doing anything now
        }

        public Meeting GetMeetingById(string id)
        {
            return _context.Meetings.FirstOrDefault(p => p.UniqueId == id);
        }


        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void DeleteMeeting(Meeting meeting)
        {
            _context.Meetings.Remove(meeting);
        }

        public Weather GetWeather(string city)
        {
            var client = new RestClient($"https://api.weatherapi.com/v1/current.json?key=cd543e4c763f4b40a3a92040211307&q={city}&aqi=no");
            var request = new RestRequest(Method.GET);
            request.AddParameter("text/plain", @"", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            JObject jObject = (JObject)JObject.Parse(response.Content)["current"];
            double temperature = (double)jObject["temp_c"];
            DateTime lastUpdated = (DateTime)jObject["last_updated"];
            string condition = (string)jObject["condition"]["text"];
            string conditionURL = (string)jObject["condition"]["icon"];
            double windSpeed = (double)jObject["wind_kph"];
            double cloud = (double)jObject["cloud"];
            Weather weather = new Weather(temperature, lastUpdated, condition, conditionURL, windSpeed, cloud);
            return weather;
        }
    }
}