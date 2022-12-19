using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MTCGServer.Models
{
    internal class Trade
    {
        public string Id { get; set; }
        public string CardToTrade { get; set; }
        public string Type { get; set; }
        public float MinDamage { get; set; }

        public Trade (string id, string cardToTrade, string type, float minDamage, User player)
        {
            Card? foundCard = null;

            if(player.Stack is null)
            {
                throw new TradeNotPossibleException();
            }
            //nach karte suchen
            foundCard = player.Stack.FirstOrDefault(x => x.Id == id);

            if(foundCard is null)
            {
                throw new TradeNotPossibleException();
                //user does not have this card ErrCode 403
            }
            else
            {
                if(foundCard.Fightable is false)
                {
                    throw new TradeNotPossibleException();
                }

                Id = id;
                CardToTrade = cardToTrade;
                Type = type;
                MinDamage = minDamage;

                //block foundcard to be added to the Stack
                foundCard.Fightable = false;
            }
            
        }
    }
}
