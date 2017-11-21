using System;
using System.Collections.Generic;
using RestSharp;
using RestSharp.Deserializers;
using Newtonsoft.Json;

namespace RestSharpProxyTestForDotNETFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("If you do not have a key for the Dark Sky API yet,");
            Console.WriteLine("you may register for one at https://darksky.net/dev/register.");
            Console.Write("Enter secret key: ");
            string SecretKey = Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine("Retrieving Forecast for North Pole, Alaska:");
            const double Latitude = 64.751111;
            const double Longitude = -147.351944;
            DarkSkyAPI API = new DarkSkyAPI(SecretKey);
            Forecast MyForecast = API.GetForecast(Latitude, Longitude);
            Console.WriteLine(JsonConvert.SerializeObject(MyForecast));
            Console.Write("Pres any key to continue...");
            Console.ReadKey();
            Console.WriteLine();
            Console.WriteLine("Retrieving raw forecast for North Pole, Alaska:");
            Console.WriteLine(API.GetRawResponse(Latitude, Longitude));
            Console.Write("Press any key to exit...");
            Console.ReadKey();
            Console.WriteLine();
        }
    }

    public class Alert
    {
        public string Description { get; set; }
        public int? Expires { get; set; }
        public List<string> Regions { get; set; }
        public string Severity { get; set; }
        public int? time { get; set; }
        public string title { get; set; }
        public string Uri { get; set; }
    }

    public class DataBlock
    {
        public List<DataPoint> Data { get; set; }
        public string Summary { get; set; }
        public string Icon { get; set; }
    }

    public class DataPoint
    {
        public double? ApparentTemperature { get; set; }
        public double? ApparentTemperatureMax { get; set; }
        public int? ApparentTemperatureMaxTime { get; set; }
        public double? ApparentTemperatureMin { get; set; }
        public int? apparentTemperatureMinTime { get; set; }
        public double? CloudCover { get; set; }
        public double? DewPoint { get; set; }
        public double? Humidity { get; set; }
        public string Icon { get; set; }
        public string MoonPhase { get; set; }
        public double? NearestStormBearing { get; set; }
        public double? NearestStormDistance { get; set; }
        public double? Ozone { get; set; }
        public double? PrecipAccumulation { get; set; }
        public double? PrecipIntensity { get; set; }
        public double? PrecipIntensityMax { get; set; }
        public int? PrecipIntensityMaxTime { get; set; }
        public double? PrecipProbability { get; set; }
        public string PrecipType { get; set; }
        public double? Pressure { get; set; }
        public string Summary { get; set; }
        public int? SunriseTime { get; set; }
        public int? SunsetTime { get; set; }
        public double? Temperature { get; set; }
        public double? TemperatureMax { get; set; }
        public int? TemperatureMaxTime { get; set; }
        public double? TemperatureMin { get; set; }
        public int? TemperatureMinTime { get; set; }
        public string Time { get; set; }
        [DeserializeAs(Name = "uvIndex")]
        public double? UVIndex { get; set; }
        public int? UVIndexTime { get; set; }
        public double? Visibility { get; set; }
        public double? WindBearing { get; set; }
        public double? WindGust { get; set; }
        public int? WindGustTime { get; set; }
        public double? WindSpeed { get; set; }
    }

    public class Flags
    {
        public string DarkSkyUnavailable { get; set; }
        public List<string> Sources { get; set; }
        [DeserializeAs(Name = "isd-stations")]
        public List<string> ISDStations { get; set; }
        public string Units { get; set; }
    }

    public class Forecast
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string TimeZone { get; set; }
        public DataPoint Currently { get; set; }
        public DataBlock Minutely { get; set; }
        public DataBlock Hourly { get; set; }
        public DataBlock Daily { get; set; }
        public List<Alert> Alerts { get; set; }
        public Flags Flags { get; set; }
    }

    public class DarkSkyAPI
    {
        const string BaseUrl = "https://api.darksky.net/forecast/";
        string SecretKey { get; set; }

        public DarkSkyAPI(string SecretKey)
        {
            this.SecretKey = SecretKey;
        }

        public T Execute<T>(RestRequest request) where T : new()
        {
            var client = new RestClient();
            client.BaseUrl = new Uri(BaseUrl);
            request.AddParameter("secretKey", SecretKey, ParameterType.UrlSegment);
            var response = client.Execute<T>(request);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response. Check inner details for more info.";
                var darkSkyException = new ApplicationException(message, response.ErrorException);
                throw darkSkyException;
            }
            return response.Data;
        }

        public Forecast GetForecast(double latitude, double longitude)
        {
            var request = new RestRequest();
            request.Resource = "{secretKey}/{latitude},{longitude}";
            request.AddParameter("latitude", latitude.ToString(), ParameterType.UrlSegment);
            request.AddParameter("longitude", longitude.ToString(), ParameterType.UrlSegment);
            request.AddParameter("secretKey", SecretKey, ParameterType.UrlSegment);
            return Execute<Forecast>(request);
        }

        public string GetRawResponse(double latitude, double longitude)
        {
            var request = new RestRequest();
            request.Resource = "{secretKey}/{latitude},{longitude}";
            request.AddParameter("latitude", latitude.ToString(), ParameterType.UrlSegment);
            request.AddParameter("longitude", longitude.ToString(), ParameterType.UrlSegment);
            var client = new RestClient();
            string BaseUrl = "https://api.darksky.net/forecast/";
            client.BaseUrl = new Uri(BaseUrl);
            request.AddParameter("secretKey", SecretKey, ParameterType.UrlSegment);
            IRestResponse response = client.Execute(request);
            return response.Content;

        }
    }

}
