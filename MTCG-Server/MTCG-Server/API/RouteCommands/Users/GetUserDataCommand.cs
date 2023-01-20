using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MTCGServer.BLL;
using MTCGServer.BLL.Exceptions;
using MTCGServer.Core.Response;
using MTCGServer.Core.Routing;
using MTCGServer.Models;
using Newtonsoft.Json;

namespace MTCGServer.API.RouteCommands.Users
{
    internal class GetUserDataCommand : ICommand
    {
        private readonly IUserManager _userManager;
        private readonly User _user;
        private readonly string _usernameToGetData;

        public GetUserDataCommand(IUserManager userManager, User user, string username)
        {
            _userManager = userManager;
            _user = user;
            _usernameToGetData = username;
        }
        public Response Execute()
        {
            var response = new Response();
            UserData? userdata = null;
            //GetTokenOfUsername wirft AccessTokenMissingException wenn token = null ist
            try
            {
                if (_userManager.ExistUser(_usernameToGetData))
                {
                    if (_user.Credentials.Username == "admin" || _user.Token == _userManager.GetTokenOfUsername(_usernameToGetData))
                    {
                        userdata = _userManager.GetUserData(_user.Credentials.Username);
                        if (userdata == null)
                        {
                            response.StatusCode = StatusCode.InternalServerError;
                        }
                        else
                        {
                            response.StatusCode = StatusCode.Ok;
                            response.Payload = JsonConvert.SerializeObject(userdata, Formatting.Indented);
                        }
                    }
                    else
                    {
                        response.StatusCode = StatusCode.Unauthorized;
                    }
                }
                else
                {
                    //User not found because not existing 
                    response.StatusCode = StatusCode.NotFound;
                }
            }
            catch (DataAccessException)
            {
                response.StatusCode = StatusCode.InternalServerError;
            }

            return response;
        }
    }
}
