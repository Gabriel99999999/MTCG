// See https://aka.ms/new-console-template for more information
using System.Linq.Expressions;

/*Console.WriteLine("Hello, World!");
User player1 = new User("Gabriel", "123");
Store store = new Store();
try { 
    store.addTrade(new Trade("djkladfafd", "jfajskf", "monster", 5, player1));
}
catch(TradeNotPossibleException){
    //The deal contains a card that is not owned by the user or locked in the deck. 403
}*/

using System.Net;
using MTCGServer.API.RouteCommands;
using MTCGServer.BLL;
using MTCGServer.Core.Server;
using MTCGServer.DAL;

string connectionString = "Server=localhost;Port=10002;User Id=postgres;Password=123;Database=MTCGServer;";
var database = new Database(connectionString);
var userDao = database.UserDao;
var packageDao = database.PackageDao;
var cardDao = database.CardDao;
var gameDao = database.GameDao;
var tradeDao = database.TradeDao;

var userManager = new UserManager(userDao);
var packageManager = new PackageManager(packageDao);
var cardsManager = new CardManager(cardDao);
var gameManager = new GameManager(gameDao);
var tradingManager = new TradingManager(tradeDao);

Lobby lobby = new Lobby();
Router router = new Router(userManager, packageManager, cardsManager, gameManager, tradingManager, lobby);
HttpServer server = new HttpServer(IPAddress.Any, 10001, router);

server.Start();