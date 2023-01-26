using MTCGServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGServer.DAL.Game
{
    public interface IGameDao
    {
        //ScoreboardData? GetIndividuelScoreboardData(User user);
        List<ScoreboardData> GetScoreboard();
        bool UpdateElo(User user);
    }
}
