using MTCGServer.Core.Response;
using MTCGServer.Core.Routing;
using MTCGServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGServer.API.RouteCommands
{
    public abstract class AuthenticatedRouteCommand : ICommand
    {
        protected User _user;

        public AuthenticatedRouteCommand(User identity)
        {
            _user = identity;
        }

        public abstract Response Execute();
    }
}
