
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelstraApp.Core.Models;

namespace TelstraApp.Core.Interfaces
{
    public interface IAzureDB
    {
        MobileServiceClient GetMobileServiceClient();
    }
}
