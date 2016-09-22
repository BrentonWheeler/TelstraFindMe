using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvvmCrossDemo.Core.Models
{
    public class ForecastWrapper : Forecast
    {
        public ForecastWrapper(Forecast forecast, string localizedName)
        {
            DailyForecasts = forecast.DailyForecasts;
            HeadLine = forecast.HeadLine;
            LocalizedName = localizedName;
        }
        public string LocalizedName { get; set; }
    }
}
