using MTCGServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGServer.DAL.Packages
{
    internal class MemoryPackageDao : IPackageDao
    {
        public bool AddPackage(Package package)
        {
            throw new NotImplementedException();
        }

        public List<Card>? BuyPackage(User user)
        {
            throw new NotImplementedException();
        }
    }
}
