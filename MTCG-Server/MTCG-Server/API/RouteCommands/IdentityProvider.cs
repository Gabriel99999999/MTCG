using MTCGServer.BLL;
using MTCGServer.Core.Request;
using MTCGServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MTCGServer.API.RouteCommands
{
    internal class IdentityProvider : IIdentityProvider
    {
        private readonly IUserManager _userManager;

        public IdentityProvider(IUserManager userManager)
        {
            _userManager = userManager;
        }

        public User? GetIdentityForRequest(Request request)
        {
            User? currentUser = null;

            if (request.Header.TryGetValue("Authorization", out var authToken))
            {
                const string prefix = "Bearer ";
                if (authToken.StartsWith(prefix))
                {
                    try
                    {
                        currentUser = _userManager.GetUserByAuthToken(authToken.Substring(prefix.Length));
                    }
                    catch(UserNotFoundException) {
                        throw;
                    }
                }
                else
                {
                    throw new AccessTokenMissingException();
                }
            }
            else
            {
                throw new AccessTokenMissingException();
            }

            return currentUser;
        }
    }

    [Serializable]
    public class AccessTokenMissingException : Exception
    {
        public AccessTokenMissingException()
        {
        }

        public AccessTokenMissingException(string? message) : base(message)
        {
        }

        public AccessTokenMissingException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected AccessTokenMissingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
