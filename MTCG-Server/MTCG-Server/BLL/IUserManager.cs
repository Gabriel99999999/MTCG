using MTCGServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MTCGServer.BLL
{
    public interface IUserManager
    {
        User? LoginUser(Credentials credentials);
        bool RegisterUser(Credentials credentials);
        User? GetUserByAuthToken(string authToken);
        string? GetTokenOfUsername(string username);
        UserData? GetUserData(string username);
        bool ExistUser(string username);
        bool UpdateUserData(UserData userData, string username);
    }
}
