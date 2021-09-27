using System;
using System.ComponentModel.DataAnnotations;

namespace webapi.Models
{
    public class Weather
    {
        public Weather(double temp, DateTime lastUpdate, string cond, string condURL, double windSpeed, double cloudNum)
        {
            Temperature = temp;
            LastUpdated = lastUpdate;
            Condition = cond;
            ConditionIconURL = condURL;
            WindSpeed = windSpeed;
            Cloud = cloudNum;
        }

        [Required]
        public double Temperature { get; set; }
        [Required]
        public DateTime LastUpdated { get; set; }
        [Required]
        public string Condition { get; set; }
        [Required]
        public string ConditionIconURL { get; set; }
        [Required]
        public double WindSpeed { get; set; }
        [Required]
        public double Cloud { get; set; }

    }
}