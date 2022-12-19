using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGServer.Models
{
    internal class SpellCard : Card
    {
        public SpellCard(string id, string name, float damage) : base(id, name, damage) { }
    }
}
