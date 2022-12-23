// See https://aka.ms/new-console-template for more information
using MTCGServer.Models;

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

var userDao = new InMemoryUserDao();
var userManager = new UserManager(userDao);

/*var messageDao = new InMemoryMessageDao();
var messageManager = new MessageManager(messageDao);*/

var router = new Router(userManager);
var server = new HttpServer(IPAddress.Any, 10003, router);

server.Start();