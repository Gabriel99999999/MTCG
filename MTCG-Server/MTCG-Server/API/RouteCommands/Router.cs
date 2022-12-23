using Newtonsoft.Json;
using MTCGServer.API.RouteCommands.Users;
using MTCGServer.BLL;
using MTCGServer.Core.Request;
using MTCGServer.Core.Routing;
using MTCGServer.Models;
using HttpMethod = MTCGServer.Core.Request.HttpMethod;

namespace MTCGServer.API.RouteCommands
{
    internal class Router : IRouter
    {
        private readonly IUserManager _userManager;
        private readonly IdentityProvider _identityProvider;
        private readonly IRouteParser _routeParser = new IdRouteParser();

        public Router(IUserManager userManager)
        {
            _userManager = userManager;

            // better: define IIdentityProvider interface and get concrete implementation passed in as dependency
            _identityProvider = new IdentityProvider(userManager);
        }

        public ICommand? Resolve(Request request)
        {
            try
            {
                var identity = (Request request) => _identityProvider.GetIdentityForRequest(request) ?? throw new RouteNotAuthenticatedException();
                var isMatch = (string path, string pattern) => _routeParser.IsMatch(path, pattern);
                //return mir ein dictionary und im falle das ich den user haben möchte schreibe ich getParameter(path,pattern)["username"]
                var getParameter = (string path, string pattern) => (_routeParser.ParseParameters(path, pattern));

                ICommand? command = request switch
                {
                    { Method: HttpMethod.Post, ResourcePath: "/users" } => new RegisterCommand(_userManager, Deserialize<Credentials>(request.Payload)),
                    { Method: HttpMethod.Post, ResourcePath: "/sessions" } => new LoginCommand(_userManager, Deserialize<Credentials>(request.Payload)),
                    { Method: HttpMethod.Get, ResourcePath: var path } when isMatch(path, "/users/{username}") => new GetUserDataCommand(_userManager, identity(request), getParameter(path, "/users/{username}")["username"]),

                    // TODO: throw RouteNotAuthenticatedException for missing identity
                    // TODO: throw InvalidDataException for missing payload

                    _ => null

                };
                return command;

            }
            catch(Exception ex) 
            {
                if (ex is UserNotFoundException) { throw; }
                else if (ex is InvalidOperationException) { throw; }
                else if (ex is RouteNotAuthenticatedException) { throw; }
                else if (ex is AccessTokenMissingException) { throw; }
                else if (ex is NotImplementedException) { throw; }
                else if (ex is InvalidDataException) { throw; }
                else { throw; }
            }
            

        }

        private T Deserialize<T>(string? body) where T : class
        {
            var data = body != null ? JsonConvert.DeserializeObject<T>(body) : null;
            if (data == null)
            {
                throw new InvalidDataException();
            }
            return data;
        }
    }
}
