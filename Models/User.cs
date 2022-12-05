using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Models
{
    internal class User
    {
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string Token => $"{Username}-msgToken";

        public List<Card> Stack { get; set; }
        public List<Card> Deck { get; set; }

        public User(string username, string password)
        {
            Username = username;
            Password = password;
        }

    }
}
