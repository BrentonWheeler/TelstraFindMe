using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelstraApp.Core.Models
{
    public class BuildLocationMessage
    {
        public string message { set; get; }
        public BuildLocationMessage(Users responseMessage)
        {
            string msg = "Hi there,\n";
            if (responseMessage.RespLunch)
                msg += "I'm an currently at lunch.\n";
            else if (responseMessage.RespMeeting)
            {
                msg += "I'm an currently in a meeting";
                if (responseMessage.RespRoom != null && responseMessage.RespRoom != "")
                {
                    msg += " in room " + responseMessage.RespRoom + "\n";
                }
                else
                {
                    msg += ".\n";
                }
            }
            else if (responseMessage.RespHome)
            {
                msg += "I'm an currently at home today.\n";
            }

            if (responseMessage.RespLocationLat != 0 && responseMessage.RespLocationLng != 0)
            {
                msg += "\nYou can find me at the below location\n";
            }
          
            message = msg;
        }
       
    
    }
}
