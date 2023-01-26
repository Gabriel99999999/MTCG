using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGServer.Models
{
    public class UserData
    {
        public string Name { get; set; }
        public string Bio { get; set; }
        public string Image { get; set; }

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
