using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Bussines.HubConfig;
using Entities;
using Microsoft.AspNetCore.SignalR;

namespace Bussines.Services
{
    public class WeatherForecastService : IWeatherForecastService
    {
        private readonly IHubContext<WeatherForecastHub> _hub;
        // Define the array to store latitude and longitude values
        Tuple<double, double>[] coordinates = new Tuple<double, double>[50];

        // Hypothetical starting latitude and longitude
        double startLatitude = 40.795048;
        double startLongitude = 29.419874;
        public int counter = 0;

        public WeatherForecastService(IHubContext<WeatherForecastHub> hub)
        {
            _hub = hub;
        }

        private static Random random = new Random();

        public double latitude;
        public double longitude;
        public async Task GetData()
        {
            // ... existing code ...
            // Generate latitude and longitude values second-by-second
            for (int i = 0; i < 50; i++)
            {
                // Increment latitude and longitude for each second
                double latitude = startLatitude + (i * 0.0001); // Example increment
                double longitude = startLongitude - (i * 0.0001); // Example decrement

                // Store latitude and longitude in the array
                coordinates[i] = Tuple.Create(latitude, longitude);
            }

            var timer = new System.Timers.Timer(1000); // 1 second interval
            timer.Elapsed += TimerOnElapsed;
            timer.Start();
        }

        private void TimerOnElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // Update latitude and longitude
            latitude = coordinates[counter].Item1;
            longitude = coordinates[counter].Item2;

            counter++;
            if(counter == 50) { counter = 0; }


            // Create location object (modify if needed)
            var location = new { lat = latitude, lon = longitude };

            // Send data to SignalR clients
            _hub.Clients.All.SendAsync("transferdata", location);
        } 
    }
}
