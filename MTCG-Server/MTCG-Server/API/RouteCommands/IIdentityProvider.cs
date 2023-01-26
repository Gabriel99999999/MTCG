using MTCGServer.BLL;
using MTCGServer.Core.Request;
using MTCGServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGServer.API.RouteCommands
{
    public interface IIdentityProvider
    {       
        public User? GetIdentityForRequest(Request request);
    }
}
