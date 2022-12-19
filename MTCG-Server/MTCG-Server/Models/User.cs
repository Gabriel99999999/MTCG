using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGServer.Models
{
    internal class User
    {
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string token => $"{Username}-msgToken";
        public string Name { get; private set; }
        public string Bio { get; private set; }
        public string Image { get; private set; }

        public int Money { get; }

        public List<Card>? Stack { get; set; }
        public List<Card>? Deck { get; set; }

        public User(string username, string password)
        {
            Username = username;
            Password = password;
            Money = 20;
        }

    }
}
