using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Models
{
    public enum CardType
    {
        //haben nur ein Element
        Knight,
        Dragon,
        Kraken,
        Wizzard,
        Ork,
        //können von jedem Element Typ sein
        Goblin,
        Elve,
        Troll,
        WaterSpell,
        FireSpell,
        RegularSpell
    }
}
