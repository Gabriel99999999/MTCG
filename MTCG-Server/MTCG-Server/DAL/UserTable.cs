using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGServer.DAL
{
    internal enum UserTable
    {
        username = 0,
        password,
        money,
        name,
        bio,
        image,
        win,
        loss,
        elo,
        token
    }
}
