using MTCGServer.BLL.Exceptions;
using MTCGServer.DAL.Exceptions;
using MTCGServer.DAL.Game;
using MTCGServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGServer.BLL
{
    public class GameManager : IGameManager
    {
        private readonly IGameDao _gameDao;
        public GameManager(IGameDao gameDao)
        {
            _gameDao = gameDao;
        }

        public List<ScoreboardData> GetScoreboard()
        {
            try
            {
                return _gameDao.GetScoreboard();
            }
            catch(Exception ex)
            {
                if(ex is DataAccessFailedException)
                {
                    throw new DataAccessException();
                }
                else
                {
                    throw;
                }
            }
        }

        public bool UpdateElo(User user)
        {
            try
            {
                return _gameDao.UpdateElo(user);
            }
            catch (Exception ex)
            {
                if (ex is DataAccessFailedException) { throw new DataAccessException(); }
                else { throw; }
            }
        }

        /*public ScoreboardData? GetIndividuelScoreboardData(User user)
        {
            return _gameDao.GetIndividuelScoreboardData(user);
        }*/
    }
}
