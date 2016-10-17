using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelstraApp.Core.Models
{
    public class TimeFormatter
    {
        public string reqTime { get; set; }


        public TimeFormatter()
        {
            DateTime dt = DateTime.Now;
            ConvertTime(dt);
        }

        public TimeFormatter(DateTime usertime)
        {

            ConvertTime(usertime);

            // DateTime dt = DateTime.ParseExact(usertime.ToString(), "dd/mm/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
            // curTime = dt.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
        }

        public void ConvertTime(DateTime usertime)
        {
            DateTime currTime = DateTime.Now;
            if ((currTime - usertime).TotalDays == 1)
            {
                reqTime = "Yesterday";

            }
            else if ((currTime - usertime).TotalDays > 1)
            {
                reqTime = usertime.Date.ToString("d");
            }
            else
            {
                reqTime = usertime.ToString("hh:mm tt");
            }

        }
    }
}
