using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGServer.Models
{
    internal class SimpleCard
    {
        public string Name;
        public string Damage;
        public string Id;

        public SimpleCard(string id, string name, string damage)
        {
            Id = id;
            Name = name;
            Damage = damage;
        }
    }

}

