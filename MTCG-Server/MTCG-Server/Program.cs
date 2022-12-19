// See https://aka.ms/new-console-template for more information
using MTCGServer.Models;

using System.Linq.Expressions;

Console.WriteLine("Hello, World!");
User player1 = new User("Gabriel", "123");
Store store = new Store();
try { 
    store.addTrade(new Trade("djkladfafd", "jfajskf", "monster", 5, player1));
}
catch(TradeNotPossibleException){
    //The deal contains a card that is not owned by the user or locked in the deck. 403
}