
//using MTCGServer.BLL;
using MTCGServer.Core.Response;
using MTCGServer.Core.Routing;
using MTCGServer.Models;

namespace MTCGServer.API.RouteCommands.Users
{
    internal class RegisterCommand : IRouteCommand
    {
        private readonly Credentials _credentials;
        private readonly IUserManager _userManager;

        public RegisterCommand(IUserManager userManager, Credentials credentials)
        {
            _credentials = credentials;
            _userManager = userManager;
        }

        public Response Execute()
        {
            var response = new Response();
            try
            {
                _userManager.RegisterUser(_credentials);
                response.StatusCode = StatusCode.Created;
                response.Payload = "das ist ein payload";
            }
            catch(DuplicateUserException)
            {
                response.StatusCode = StatusCode.Conflict;
            }
            return response;
        }
    }
}
