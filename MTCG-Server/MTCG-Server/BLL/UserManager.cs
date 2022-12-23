using MTCGServer.DAL;
using MTCGServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGServer.BLL
{
    internal class UserManager : IUserManager
    {
        private readonly IUserDao _userDao;

        public UserManager(IUserDao userDao)
        {
            _userDao = userDao;
        }

        public User LoginUser(Credentials credentials)
        {
            return _userDao.GetUserByCredentials(credentials.Username, credentials.Password) ?? throw new UserNotFoundException();
        }

        public void RegisterUser(Credentials credentials)
        {
            var user = new User(credentials);
            if (_userDao.InsertUser(user) == false)
            {
                throw new DuplicateUserException();
            }
        }

        public User GetUserByAuthToken(string authToken)
        {
            return _userDao.GetUserByAuthToken(authToken) ?? throw new UserNotFoundException();
        }

        public UserData? GetUserData(string username)
        {
            return _userDao.GetUserData(username);
        }
        public string? GetTokenOfUsername(string username)
        {
            return _userDao.GetToken(username);
        }
    }
}
