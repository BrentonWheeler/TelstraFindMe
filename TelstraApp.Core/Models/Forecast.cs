using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvvmCrossDemo.Core.Models
{
    public class Forecast
    {
        [JsonProperty("Headline")]
        public Headline HeadLine { get; set; }
        public List<DailyForecast> DailyForecasts { get; set; }
        public class Headline
        {
            public string EffectiveDate { get; set; }
            public int EffectiveEpochDate { get; set; }
            public int Severity { get; set; }
            public string Text { get; set; }
            public string Category { get; set; }
            public string EndDate { get; set; }
            public int EndEpochDate { get; set; }
            public string MobileLink { get; set; }
            public string Link { get; set; }
        }

        public class Minimum
        {
            public double Value { get; set; }
            public string Unit { get; set; }
            public int UnitType { get; set; }
        }

        public class Maximum
        {
            public double Value { get; set; }
            public string Unit { get; set; }
            public int UnitType { get; set; }
        }

        public class Temperature
        {
            public Minimum Minimum { get; set; }
            public Maximum Maximum { get; set; }
        }

        public class Day
        {
            public int Icon { get; set; }
            public string IconPhrase { get; set; }
        }

        public class Night
        {
            public int Icon { get; set; }
            public string IconPhrase { get; set; }
        }

        public class DailyForecast
        {
            public string Date { get; set; }
            public int EpochDate { get; set; }
            public Temperature Temperature { get; set; }
            public Day Day { get; set; }
            public Night Night { get; set; }
            public List<string> Sources { get; set; }
            public string MobileLink { get; set; }
            public string Link { get; set; }
        }

    }
}
