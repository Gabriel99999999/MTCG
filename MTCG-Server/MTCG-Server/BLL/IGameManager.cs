using MTCGServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGServer.BLL
{
    internal interface IGameManager
    {
        List<ScoreboardData> GetScoreboard();
    }
}
