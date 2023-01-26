using MTCGServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGServer.BLL
{
    public interface ICardManager
    {
        List<Card> GetStack(User user);
        List<Card> GetDeck(User user);
        bool ConfigureDeck(User user, List<Guid> guids);
    }
}
