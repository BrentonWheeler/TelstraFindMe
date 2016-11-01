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
            string msg = responseMessage.ReqTo + ":\n";
            msg += responseMessage.RespCurrentlyAt + "\n";


            if (responseMessage.RespLocationLat != 0 && responseMessage.RespLocationLng != 0)
            {
                msg += "\nYou can find me at the below location";
                if (responseMessage.RespRoom != "")
                {
                    msg +=  " in room " + responseMessage.RespRoom;
                }
                
            } else if (responseMessage.RespRoom != "")
            {
                msg = "\n You can find me in room" + responseMessage.RespRoom;
            }
            msg += "\n";
            message = msg;
        }
       
    
    }
}
