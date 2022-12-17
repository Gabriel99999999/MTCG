using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Models
{
    internal class Trade
    {
        public string Id { get; set; }
        public string CardToTrade { get; set; }
        public string Type { get; set; }
        public float MinDamage { get; set; }

        public Trade(string id, string cardToTrade, string type, float minDamage)
        {
            Id = id;
            CardToTrade = cardToTrade;
            Type = type;
            MinDamage = minDamage;
        }
    }
}
