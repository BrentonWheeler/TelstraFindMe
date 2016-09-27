using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelstraApp.Core.Services
{
    public static class WeatherApp
    {
        public static string ApiKey = "Aooa3DdZXWk4Lnlm0L3IBMGKEfJZz7Cp";
        public static string AutoCompleteEndpoint = "http://dataservice.accuweather.com/locations/v1/cities/autocomplete";
        public static string ForecastEndpoint = "http://dataservice.accuweather.com/forecasts/v1/daily/1day/";
    }
}
