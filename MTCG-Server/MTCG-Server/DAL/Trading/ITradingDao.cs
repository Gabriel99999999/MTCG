using MTCGServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGServer.DAL.Trading
{
    public interface ITradingDao
    {
        bool CreateTrade(Trade trade, User user);
        bool DeleteTrade(Guid tradeId, string username);
        List<Trade> GetTrades();
        bool MakeTrade(Guid tradeId, Guid cardId, User user);
    }
}
