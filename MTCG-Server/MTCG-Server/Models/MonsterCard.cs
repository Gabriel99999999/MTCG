﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGServer.Models
{
    public class MonsterCard : Card
    {
        public MonsterCard(Guid id, string name, decimal damage) : base(id, name, damage) { }
    }
}
