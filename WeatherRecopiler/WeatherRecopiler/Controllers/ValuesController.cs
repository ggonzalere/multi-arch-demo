using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WeatherRecopiler.Controllers
{
    public class WeatherForecast {
        public string city { get; set; }
        public double temperature { get; set; }
        public string status { get; set; }
        public string timeStamp { get; set; }
    }

    public class WorldForecast {
        public WeatherForecast[] forecasts { get; set; }
    }
  
    public class ValuesController : Controller
    {
        public string[] cities = {"Redmond",
            "Kyoto",
            "Miami",
            "Mexico City",
            "Seattle",
            "Moscow",
            "Berlin",
            "Warsaw",
            "Bogota",
            "Buenos Aires"};

        [HttpGet("getWeather")]
        public IActionResult Get()
        {
            Random rnd = new Random();
            List<WeatherForecast> forecasts = new List<WeatherForecast>();
            foreach (string city in cities) {
                var temp = (rnd.NextDouble() * 50) - 10;
                forecasts.Add(new WeatherForecast { city = city, temperature = temp, status = getStatus(temp), timeStamp = DateTime.Now.ToString() });
            }

            return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(new WorldForecast { forecasts=forecasts.ToArray()}));
        }

        public string getStatus(double temperature) {
            if (temperature < 0) {
                return "extremely cold";
            }
            if (temperature < 10) {
                return "cold";
            }
            if (temperature < 20) {
                return "mild";
            }
            if (temperature < 30) {
                return "hot";
            }
            return "extremely hot";
        }

    }
}
