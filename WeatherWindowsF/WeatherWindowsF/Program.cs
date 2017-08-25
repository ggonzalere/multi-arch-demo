using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading;
using Newtonsoft.Json;

namespace WeatherWindowsF
{
    class Program
    {
        public class WeatherForecast
        {
            public string city { get; set; }
            public double temperature { get; set; }
            public string status { get; set; }
            public string timeStamp { get; set; }
        }

        public class WorldForecast
        {
            public WeatherForecast[] forecasts { get; set; }
        }
        static void Main(string[] args)
        {
            Timer timer = new Timer(callback, null, 0, 5000);
            Console.ReadLine();
        }

        private static void callback(Object state)
        {
            GetWeather().Wait();
        }
        private static async Task GetWeather()
        {
            Console.WriteLine("Container on windows V1");
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            var stringTask = client.GetStringAsync("yourUrl");
            var msg = await stringTask;
            var wf = JsonConvert.DeserializeObject<WorldForecast>(msg);
            Console.Write(getExtremes(wf));
        }

        public static String getExtremes(WorldForecast wf)
        {
            String cha = "At " + wf.forecasts[0].timeStamp + ":\n";
            for (int i = 0; i < wf.forecasts.Length; i++)
            {
                if (wf.forecasts[i].status.Equals("extremely cold") || wf.forecasts[i].status.Equals("extremely hot"))
                {
                    cha = cha + wf.forecasts[i].city + " is " + wf.forecasts[i].status + " with " + toFahrenheit(wf.forecasts[i].temperature) + " Fahrenheit\n";
                }
            }
            cha = cha + "\n";
            return cha;
        }

        public static double toFahrenheit(double c)
        {
            return (9.0 / 5.0) * c + 32;
        }
    }
}