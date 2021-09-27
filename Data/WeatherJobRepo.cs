using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using webapi.Data;
using webapi.Models;

namespace Hangfire.Demo
{
    public class WeatherJobRepo : IWeatherJobRepo
    {
        private readonly IServiceScopeFactory scopeFactory;
        public WeatherJobRepo(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        public void ControlWeather()
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<IContext>();
                Dictionary<string, Weather> cityNames = new Dictionary<string, Weather>();
                foreach (Meeting meeting in context.Meetings)
                {
                    if (!cityNames.ContainsKey(meeting.UniqueId))
                    {
                        // var client = new RestClient($"https://api.weatherapi.com/v1/current.json?key=cd543e4c763f4b40a3a92040211307&q={meeting.City}&aqi=no");
                        // var request = new RestRequest(Method.GET);
                        // request.AddParameter("text/plain", @"", ParameterType.RequestBody);
                        // IRestResponse response = client.Execute(request);
                        // JObject jObject = (JObject)JObject.Parse(response.Content)["current"];
                        // double temperature = (double)jObject["temp_c"];
                        // DateTime lastUpdated = (DateTime)jObject["last_updated"];
                        // string condition = (string)jObject["condition"]["text"];
                        // string conditionURL = (string)jObject["condition"]["icon"];
                        // double windSpeed = (double)jObject["wind_kph"];
                        // double cloud = (double)jObject["cloud"];
                        // Weather weather = new Weather(temperature, lastUpdated, condition, conditionURL, windSpeed, cloud);
                        double temperature = 10;
                        DateTime lastUpdated = DateTime.MaxValue;
                        string condition = "asd";
                        string conditionURL = "asd";
                        double windSpeed = 10;
                        double cloud = 10;
                        Weather weather = new Weather(temperature, lastUpdated, condition, conditionURL, windSpeed, cloud);
                        cityNames.Add(meeting.UniqueId, weather);
                    }
                }

                foreach (Participance participance in context.Participances)
                {
                    if (cityNames.ContainsKey(participance.MeetingId))
                    {
                        Weather weather = cityNames[participance.MeetingId];
                    }
                    // TODO: İSTEK GİDECEK
                    // if (temperature > 30)
                    // {
                    // WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                    // tRequest.Method = "post";
                    // tRequest.Headers.Add(string.Format("Authorization: key={0}", "AIXXXXXX...."));
                    // tRequest.Headers.Add(string.Format("Sender: id={0}", "XXXXX.."));
                    // tRequest.ContentType = "application/json";
                    // var payload = new
                    // {
                    //     to = "e8EHtMwqsZY:APA91bFUktufXdsDLdXXXXXX..........XXXXXXXXXXXXXX",
                    //     priority = "high",
                    //     content_available = true,
                    //     notification = new
                    //     {
                    //         body = "Test",
                    //         title = "Test",
                    //         badge = 1
                    //     },
                    //     data = new
                    //     {
                    //         key1 = "value1",
                    //         key2 = "value2"
                    //     }
                    // };

                    // string postbody = JsonConvert.SerializeObject(payload).ToString();
                    // Byte[] byteArray = Encoding.UTF8.GetBytes(postbody);
                    // tRequest.ContentLength = byteArray.Length;
                    // using (Stream dataStream = tRequest.GetRequestStream())
                    // {
                    //     dataStream.Write(byteArray, 0, byteArray.Length);
                    //     using (WebResponse tResponse = tRequest.GetResponse())
                    //     {
                    //         using (Stream dataStreamResponse = tResponse.GetResponseStream())
                    //         {
                    //             if (dataStreamResponse != null) using (StreamReader tReader = new StreamReader(dataStreamResponse))
                    //                 {
                    //                     String sResponseFromServer = tReader.ReadToEnd();
                    //                     //result.Response = sResponseFromServer;
                    //                 }
                    //         }
                    //     }
                    // }
                    // Console.WriteLine("Çok Sıcak");
                    // }
                    // else
                    // {
                    //     Console.WriteLine($"Yes{weather.Condition} {weather.Temperature}");
                    // }
                }
            }
        }

        public void Print() { }
    }
}