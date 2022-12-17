using Newtonsoft.Json;
using SWE1.MessageServer.API.RouteCommands.Messages;
using SWE1.MessageServer.API.RouteCommands.Users;
using SWE1.MessageServer.BLL;
using SWE1.MessageServer.Core.Request;
using SWE1.MessageServer.Core.Routing;
using SWE1.MessageServer.Models;
using HttpMethod = SWE1.MessageServer.Core.Request.HttpMethod;

namespace SWE1.MessageServer.API.RouteCommands
{
    internal class Router : IRouter
    {
        private readonly IUserManager _userManager;
        private readonly IMessageManager _messageManager;
        private readonly IdentityProvider _identityProvider;

        public Router(IUserManager userManager, IMessageManager messageManager)
        {
            _userManager = userManager;
            _messageManager = messageManager;

            // better: define IIdentityProvider interface and get concrete implementation passed in as dependency
            _identityProvider = new IdentityProvider(userManager);
        }

        public IRouteCommand? Resolve(RequestContext request)
        {
            var identity = _identityProvider.GetIdentityForRequest(request);

            IRouteCommand? command = request switch
            {
                { Method: HttpMethod.Post, ResourcePath: "/users" } => new RegisterCommand(_userManager, Deserialize<Credentials>(request.Payload)),
                { Method: HttpMethod.Post, ResourcePath: "/sessions"} => new LoginCommand(_userManager, Deserialize<Credentials>(request.Payload)),

                // TODO: throw RouteNotAuthenticatedException for missing identity
                // TODO: throw InvalidDataException for missing payload
                { Method: HttpMethod.Post, ResourcePath: "/messages"} => new AddMessageCommand(_messageManager, identity, request.Payload),

                _ => null
            };

            return command;
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
