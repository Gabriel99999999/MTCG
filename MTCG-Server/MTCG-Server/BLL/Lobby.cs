using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCGServer.Models;

namespace MTCGServer.BLL
{
    public class Lobby
    {
        private ConcurrentQueue<User> lobby = new ConcurrentQueue<User>();
        private ConcurrentDictionary<User, string> _battleLog = new ConcurrentDictionary<User, string>();

        public Lobby() { }

        public string EnterLobby(User currentUser, ref bool updateUser)
        {
            User? waitingUser = null;
            if (lobby.TryDequeue(out waitingUser))
            {
                Battlefield battlefield = new Battlefield(currentUser, waitingUser);
                string battleLog = battlefield.StartFight(ref updateUser);
                _battleLog.TryAdd(waitingUser, battleLog);
                return battleLog;
            }
            else
            {
                lobby.Enqueue(currentUser);
                string? battleLog = null;
                while (!_battleLog.TryRemove(currentUser, out battleLog))
                {
                }
                return battleLog!;
            }
        }
    }
}
