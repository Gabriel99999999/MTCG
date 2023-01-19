using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MTCGServer.Models
{
    internal class Store
    {
        private List<Trade>? _currentTrades;
        private List<Package>? _currentPackages;

        public Store()
        {
            _currentPackages = null;
            _currentTrades = null;
        }
        public void listTrades()
        {
            if (_currentTrades == null)
            {
                //return 204;
                //The request was fine, but there are no trading deals available 204

            }
            else
            {
                int count = 1;
                foreach (Trade trade in _currentTrades){
                    Console.WriteLine($"Trade {count}:\nTrade Id: {trade.Id}\nCard Id: {trade.Id}\n wanted Card type: {trade.Type}\nminimum Damage: {trade.MinDamage} ");
                    count++;
                }
                //return 200;
                //There are trading deals available, the response contains these 200
            }
            
        }
        public void addTrade(Trade trade)
        {
            Trade? existingTrade = null;
            //check if this trade already exists
            existingTrade = _currentTrades.FirstOrDefault(x => x.Id == trade.Id);
            
            if (existingTrade is null)
            {
                _currentTrades.Add(trade);
                //Trading deal successfully created 201
            }
            else
            {
                //Trade already exist 409
            }
        }
        public void deleteTrade(Guid tradeId, User player) 
        {
            //search for this trade
            Trade? foundTrade = null;
            foundTrade = _currentTrades.FirstOrDefault(x => x.Id == tradeId);

            if (foundTrade is null) 
            {
                //The provided dealId was not found 404
            }
            //check ob die Karte um die es im Trade geht auch dem user gehört
            else
            {
                Guid id = foundTrade.CardToTrade;
                Card? foundCard = null;
                foundCard = player.Stack.FirstOrDefault(x => x.Id == id);
                if(foundCard is null)
                {
                    //The deal contains a card that is not owned by the user. 403
                }
                else
                {
                    _currentTrades.Remove(foundTrade);
                    //Trading deal successfully deleted 200
                }
            }
        }

        public void buyPackage(User player)
        {
            if (_currentPackages is null)
            {
                //No card package available for buying 404
            }
            else
            {
                Package package = _currentPackages.First();
                if (player.Money >= package.Price)
                {
                    if (player.Money >= package.Price)
                    {
                         _currentPackages.RemoveAt(0);
                        foreach(Card card in package.PackageOfCards)
                        {
                            player.Stack.Add(card);
                        }
                        //A package has been successfully bought 200
                    }
                }
                else
                {
                    //Not enough money for buying a card package 403
                }
            }
            
        }
    }
}
