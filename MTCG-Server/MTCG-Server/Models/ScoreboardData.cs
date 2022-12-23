using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGServer.Models
{
    internal class ScoreboardData
    {
        public int Win { get; private set; }
        public int Loss { get; private set; }
        public int Elo { get; private set; }

        public ScoreboardData()
        {
            Win = 0;
            Loss = 0;
            Elo = 100;
        }
        public ScoreboardData(int win, int loss, int elo)
        {
            Win = win;
            Loss = loss;
            Elo = elo;
        }
    }

}
