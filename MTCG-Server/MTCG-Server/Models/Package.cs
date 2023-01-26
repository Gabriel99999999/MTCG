using MTCGServer.Core.Request;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGServer.Models
{
    public class Package
    {
        public List<Card> PackageOfCards { get; set; }
        public int Price { get; }
        
        public Package(List<Card> package)
        {
            PackageOfCards = package;
            Price = 5;
        }
    }
}
