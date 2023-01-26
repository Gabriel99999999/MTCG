using MTCGServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGServer.BLL
{
    public interface ITradingManager
    {
        bool CreateTrade(Trade trade, User user);
        bool DeleteTrade(User user, Guid tradeId);
        List<Trade> GetTrades();
        bool MakeTrade(Guid tradeId, Guid cardId, User user);
    }
}
