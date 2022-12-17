using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Models
{
    internal class Package
    {
        public List<Card> _Package { get; }
        
        public Package(Card card1, Card card2, Card card3, Card card4)
        {
            _Package.Add(card1);
            _Package.Add(card2);
            _Package.Add(card3);
            _Package.Add(card4);
        }
    }
}
