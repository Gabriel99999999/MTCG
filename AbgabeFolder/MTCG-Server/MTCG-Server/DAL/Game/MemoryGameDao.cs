using MTCGServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGServer.DAL.Game
{
    public class MemoryGameDao : IGameDao
    {
        /*public ScoreboardData? GetIndividuelScoreboardData(User user)
        {
            throw new NotImplementedException();
        }*/
        public List<ScoreboardData> GetScoreboard()
        {
            throw new NotImplementedException();
        }

        public bool UpdateElo(User user)
        {
            throw new NotImplementedException();
        }
    }
}
