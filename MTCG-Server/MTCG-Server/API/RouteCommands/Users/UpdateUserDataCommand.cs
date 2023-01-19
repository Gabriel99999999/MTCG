using MTCGServer.BLL;
using MTCGServer.BLL.Exceptions;
using MTCGServer.Core.Response;
using MTCGServer.Core.Routing;
using MTCGServer.Models;
using Newtonsoft.Json;

namespace MTCGServer.API.RouteCommands.Users
{
    internal class UpdateUserDataCommand : ICommand
    {
        private readonly IUserManager _userManager;
        private readonly User _user;
        private readonly UserData _userData;
        private readonly string _usernameToUpdateData;

        public UpdateUserDataCommand(IUserManager userManager, User user, UserData userData, string username)
        {
            _userManager = userManager;
            _user = user;
            _userData = userData;
            _usernameToUpdateData = username;
        }

        public Response Execute()
        {
            var response = new Response();
            //GetTokenOfUsername wirft AccessTokenMissingException wenn token = null ist
            try
            {
                if (_userManager.ExistUser(_usernameToUpdateData))
                {
                    if (_user.Credentials.Username == "admin" || _user.Token == _userManager.GetTokenOfUsername(_usernameToUpdateData))
                    {
                        if (_userManager.UpdateUserData(_userData, _usernameToUpdateData))
                        {
                            response.StatusCode = StatusCode.Ok;
                        }
                        else
                        {
                            response.StatusCode = StatusCode.InternalServerError;
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