using MTCGServer.Models;
using Npgsql;

namespace MTCGServer.DAL
{
    internal interface IUserDao : IDB
    {
        User? GetUserByAuthToken(string authToken);
        User? GetUserByCredentials(string username, string password);
        bool InsertUser(User user);
        string? GetToken(string username);
        UserData? GetUserData(string username);
    }
}
