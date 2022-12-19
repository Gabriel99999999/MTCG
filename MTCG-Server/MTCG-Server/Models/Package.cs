using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGServer.Models
{
    internal class Package
    {
        public List<Card> PackageToBuy { get; set; }
        public int Price { get; }
        
        public Package(Card card1, Card card2, Card card3, Card card4, Card card5)
        {
            PackageToBuy.Add(card1);
            PackageToBuy.Add(card2);
            PackageToBuy.Add(card3);
            PackageToBuy.Add(card4);
            PackageToBuy.Add(card5);

            Price = 5;
        }
    }
}
