﻿using MTCGServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGServer.DAL
{
    internal interface ICardDao
    {
        bool ConfigureDeck(User user, List<Guid> guids);
        List<Card> GetDeck(User user);
        List<Card> GetStack(User user);
    }
}
