using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Models
{
    internal class MonsterCard : Card
    {
        public MonsterCard(string id, string name, float damage) : base(id, name, damage) { }
    }
}
