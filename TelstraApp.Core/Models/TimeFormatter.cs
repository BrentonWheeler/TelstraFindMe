using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelstraApp.Core.Models
{
    //author: Michael Kath (n9293833)
    //Simple time converter class
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
