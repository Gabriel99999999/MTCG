using Newtonsoft.Json;
using MTCGServer.API.RouteCommands.Users;
using MTCGServer.BLL;
using MTCGServer.Core.Request;
using MTCGServer.Core.Routing;
using MTCGServer.Models;
using HttpMethod = MTCGServer.Core.Request.HttpMethod;
using Npgsql.Internal;
using MTCGServer.API.RouteCommands.Packages;
using System.Collections.Generic;
using MTCGServer.API.RouteCommands.Cards;
using MTCGServer.BLL.Exceptions;

namespace MTCGServer.API.RouteCommands
{
    internal class Router : IRouter
    {
        private readonly IUserManager _userManager;
        private readonly IPackageManager _packageManager;
        private readonly ICardManager _cardManager;
        private readonly IdentityProvider _identityProvider;
        private readonly IRouteParser _routeParser = new RouteParser();

        public Router(IUserManager userManager, IPackageManager packageManager, ICardManager cardManager)
        {
            _userManager = userManager;
            _packageManager = packageManager;
            _cardManager = cardManager;

            // better: define IIdentityProvider interface and get concrete implementation passed in as dependency
            _identityProvider = new IdentityProvider(userManager);
        }

        public ICommand? Resolve(Request request)
        {
            try
            {
                var identity = (Request request) => _identityProvider.GetIdentityForRequest(request) ?? throw new RouteNotAuthenticatedException(); //returnt mir den user zum passenden token
                var isMatch = (string path, string pattern) => _routeParser.IsMatch(path, pattern); //return true if the path matches the pattern
                
                //return mir ein dictionary und im falle das ich den user haben möchte schreibe ich getParameter(path,pattern)["username"]
                var getParameter = (string path, string pattern) => (_routeParser.ParseParameters(path, pattern));

                ICommand? command = request switch
                {
                    //All Methods for the user
                    { Method: HttpMethod.Post, ResourcePath: "/users" } => new RegisterCommand(_userManager, Deserialize<Credentials>(request.Payload)),
                    { Method: HttpMethod.Post, ResourcePath: "/sessions" } => new LoginCommand(_userManager, Deserialize<Credentials>(request.Payload)),
                    { Method: HttpMethod.Get, ResourcePath: var path } when isMatch(path, "/users/{username}") => new GetUserDataCommand(_userManager, identity(request), getParameter(path, "/users/{username}")["username"]),
                    { Method: HttpMethod.Put, ResourcePath: var path } when isMatch(path, "/users/{username}") => new UpdateUserDataCommand(_userManager, identity(request), Deserialize<UserData>(request.Payload), getParameter(path, "/users/{username}")["username"]),

                    //All Methods for the packages
                    { Method: HttpMethod.Post, ResourcePath: "/packages"} => new CreatePackageCommand(_packageManager, Deserialize<List<Card>>(request.Payload), identity(request)), //if no token was set  => Response.StatusCode = 401 but if token is set and it exist => false user tries to create packages => Response.StatusCode = 403
                    { Method: HttpMethod.Post, ResourcePath: "/transactions/packages" } => new AcquirePackageCommand(_packageManager, identity(request)),

                    //All Methods for the cards
                    { Method: HttpMethod.Get, ResourcePath: "/cards"} => new GetStackCommand(_cardManager, identity(request)),
                    { Method: HttpMethod.Get, ResourcePath: "/deck" } => new GetDeckCommand(_cardManager, identity(request), request),
                    { Method: HttpMethod.Put, ResourcePath: "/deck"} => new ConfigureDeckCommand(_cardManager, identity(request), Deserialize<List<Guid>>(request.Payload)),

                    //All Methods for the game
                    //{ Method: HttpMethod.Get, ResourcePath: "/stats"} => new GetStatsCommand(_gameManager, identity(request))),
                    // TODO: throw RouteNotAuthenticatedException for missing identity
                    // TODO: throw InvalidDataException for missing payload

                    _ => null

                };
                return command;

            }
            catch(Exception ex) 
            {
                if (ex is UserNotFoundException) { throw new NotFoundException(); }
                else if (ex is InvalidOperationException) { throw; }
                else if (ex is RouteNotAuthenticatedException) { Console.WriteLine("StatusCode should be 401");  throw;  }
                else if (ex is NotImplementedException) { throw; }
                else if (ex is InvalidDataException) { throw; }
                else if (ex is JsonSerializationException) { throw new InvalidOperationException(); }
                else { throw; }
            }
            

        }

        private T Deserialize<T>(string? body) where T : class
        {
            try
            {
                var data = body != null ? JsonConvert.DeserializeObject<T>(body) : null;
                if (data == null)
                {
                    throw new InvalidDataException();
                }
            
                return data;
            }
            catch (Exception ex)
            {
                if(ex is InvalidDataException)
                {
                    throw;
                }
                if(ex is JsonSerializationException)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
                else
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
            
        }
    }
}
