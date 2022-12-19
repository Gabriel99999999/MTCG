//using MTCGServer.BLL;
using MTCGServer.Core.Response;
using MTCGServer.Core.Routing;
using MTCGServer.Models;

namespace MTCGServer.API.RouteCommands.Users
{
    internal class LoginCommand : IRouteCommand
    {
        private readonly IUserManager _userManager;

        private readonly Credentials _credentials;

        public LoginCommand(IUserManager userManager, Credentials credentials)
        {
            _credentials = credentials;
            _userManager = userManager;
        }

        public Response Execute()
        {
            User? user;
            try
            {
                user = _userManager.LoginUser(_credentials);
            }
            catch (UserNotFoundException)
            {
                user = null;
            }

            var response = new Response();
            if (user == null)
            {
                response.StatusCode = StatusCode.Unauthorized;
            }
            else
            {
                response.StatusCode = StatusCode.Ok;
                response.Payload = user.Token;
            }

            return response;
        }
    }
}
