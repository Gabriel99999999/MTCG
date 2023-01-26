using Newtonsoft.Json;
using MTCGServer.API.RouteCommands.Users;
using MTCGServer.BLL;
using MTCGServer.Core.Request;
using MTCGServer.Core.Routing;
using MTCGServer.Models;
using HttpMethod = MTCGServer.Core.Request.HttpMethod;
using MTCGServer.API.RouteCommands.Packages;
using MTCGServer.API.RouteCommands.Cards;
using MTCGServer.BLL.Exceptions;
using MTCGServer.API.RouteCommands.Game;
using MTCGServer.API.RouteCommands.Trading;

namespace MTCGServer.API.RouteCommands
{
    public class Router : IRouter
    {
        private readonly IUserManager _userManager;
        private readonly IPackageManager _packageManager;
        private readonly ICardManager _cardManager;
        private readonly IGameManager _gameManager;
        private readonly ITradingManager _tradingManager;

        private readonly IdentityProvider _identityProvider;
        private readonly IRouteParser _routeParser = new RouteParser();
        private Lobby _lobby;

        public Router(IUserManager userManager, IPackageManager packageManager, ICardManager cardManager, IGameManager gameManager, ITradingManager tradingManager, Lobby lobby)
        {
            _userManager = userManager;
            _packageManager = packageManager;
            _cardManager = cardManager;
            _gameManager = gameManager;
            _tradingManager = tradingManager;
            _lobby = lobby;

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
                    { Method: HttpMethod.Post, ResourcePath: "/packages" } => new CreatePackageCommand(_packageManager, Deserialize<List<Card>>(request.Payload), identity(request)), //if no token was set  => Response.StatusCode = 401 but if token is set and it exist => false user tries to create packages => Response.StatusCode = 403
/*Special feature:*/{ Method: HttpMethod.Post, ResourcePath: "/automatedPackages" } => new CreateAutomaticPackagesCommand(_packageManager, identity(request), Deserialize<string>(request.Payload)),          
                    { Method: HttpMethod.Post, ResourcePath: "/transactions/packages" } => new AcquirePackageCommand(_packageManager, identity(request)),

                    //All Methods for the cards
                    { Method: HttpMethod.Get, ResourcePath: "/cards" } => new GetStackCommand(_cardManager, identity(request)),
                    { Method: HttpMethod.Get, ResourcePath: var path } when isMatch(path, "/deck{query}") => new GetDeckCommand(_cardManager, identity(request), request),
                    { Method: HttpMethod.Put, ResourcePath: "/deck" } => new ConfigureDeckCommand(_cardManager, identity(request), Deserialize<List<Guid>>(request.Payload)),

                    //All Methods for the game
                    { Method: HttpMethod.Get, ResourcePath: "/stats" } => new GetStatsCommand(_gameManager, identity(request)),
                    { Method: HttpMethod.Get, ResourcePath: "/score" } when (identity(request) != null) => new GetScoreboardCommand(_gameManager),
                    { Method: HttpMethod.Post, ResourcePath: "/battles" } => new EnterLobyCommand(_gameManager, _cardManager, identity(request), _lobby),

                    //All Methods for the trading
                    { Method: HttpMethod.Get, ResourcePath: "/tradings" } => new GetTradesCommand(_tradingManager, identity(request)),
                    { Method: HttpMethod.Post, ResourcePath: "/tradings" } => new CreateTradeCommand(_tradingManager, identity(request), Deserialize<Trade>(request.Payload)),
                    { Method: HttpMethod.Delete, ResourcePath: var path } when isMatch(path, "/tradings/{id}") => new DeleteExistingTradeCommand(_tradingManager, identity(request), getParameter(path, "/tradings/{id}")["id"]),
                    { Method: HttpMethod.Post, ResourcePath: var path } when isMatch(path, "/tradings/{id}") => new MakeTradeCommand(_tradingManager, identity(request), getParameter(path, "/tradings/{id}")["id"], Deserialize<string>(request.Payload)),


                    _ => null

                };
                return command;

            }
            catch(Exception ex) 
            {
                Console.WriteLine(ex.Message);
                if (ex is DataNotFoundException) { throw new NotFoundException(); }
                else if (ex is InvalidOperationException) { throw; }
                else if (ex is RouteNotAuthenticatedException) { throw;  }
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
                
                if (ex is InvalidDataException)
                {
                    Console.WriteLine("hier");
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
