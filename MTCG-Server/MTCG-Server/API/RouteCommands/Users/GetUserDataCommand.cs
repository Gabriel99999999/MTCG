using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MTCGServer.BLL;
using MTCGServer.Core.Response;
using MTCGServer.Core.Routing;
using MTCGServer.Models;
using System.Text.Json;

namespace MTCGServer.API.RouteCommands.Users
{
    internal class GetUserDataCommand : ICommand
    {
        private readonly IUserManager _userManager;
        private readonly User _user;
        private readonly string _username;

        public GetUserDataCommand(IUserManager userManager, User user, string username)
        {
            _userManager = userManager;
            _user = user;
            _username = username;
        }
        public Response Execute()
        {
            var response = new Response();
            UserData? userdata = null;
            if (_user.Credentials.Username == "admin" || _user.Token == _userManager.GetTokenOfUsername(_username))
            {
                userdata = _userManager.GetUserData(_user.Credentials.Username);
                
            }

            
            if (userdata == null)
            {
                response.StatusCode = StatusCode.InternalServerError;
            }
            else
            {
                response.StatusCode = StatusCode.Ok;
                response.Payload = JsonSerializer.Serialize(userdata);
            }

            return response;
        }
    }
}
