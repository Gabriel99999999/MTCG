using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGServer.Models
{
    internal class UserData
    {
        public string Name { get; private set; }
        public string Bio { get; private set; }
        public string Image { get; private set; }

        //beim ersten mal anlegen von einem User
        public UserData()
        {
            Name = "";
            Bio = "";
            Image = "";
        }

        //beim aus der Datenbank auslesen für Scoreboard vermutlich notwendig
        public UserData(string name, string bio, string image)
        {
            Name = name;
            Bio = bio;  
            Image = image;
        }
    }
}
