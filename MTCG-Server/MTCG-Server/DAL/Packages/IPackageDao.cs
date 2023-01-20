using MTCGServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGServer.DAL.Packages
{
    internal interface IPackageDao
    {
        bool AddPackage(Package package);
        List<Card>? BuyPackage(User user);
    }
}
