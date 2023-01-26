using MTCGServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGServer.BLL
{
    public interface IGameManager
    {
        List<ScoreboardData> GetScoreboard();
        bool UpdateElo(User user);
    }
}
