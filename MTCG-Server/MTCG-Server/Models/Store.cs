using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Models
{
    internal class Store
    {
        private List<Trade>? currentTrades;
        private List<Package>? currentPackages;

        public int listTrades()
        {
            if (currentTrades == null)
            {
                return 204;
            }
            int count = 1;
            foreach (Trade trade in currentTrades){
                Console.WriteLine($"Trade {count}.    ");
                count++;
            }
            return 200;
        }
        public void addTrade(Trade trade)
        {
            currentTrades.Add(trade);
        }
    }
}
