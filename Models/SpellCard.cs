using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Models
{
    internal class SpellCard : Card
    {
        public SpellCard(ElementType type, CardType name, int damage)
        {
            Element = type;
            Name = name;    
            Damage = damage;
        }
    }
}
