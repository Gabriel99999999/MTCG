using MTCGServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGServer.DAL
{
    internal class MemoryCardDaocs : ICardDao
    {
        public bool ConfigureDeck(User user, List<Guid> guids)
        {
            throw new NotImplementedException();
        }

        public List<Card> GetDeck(User user)
        {
            throw new NotImplementedException();
        }

        public List<Card> GetStack(User user)
        {
            throw new NotImplementedException();
        }
    }
}
