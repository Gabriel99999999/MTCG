using MTCGServer.BLL;
using MTCGServer.Core.Request;
using MTCGServer.Core.Routing;
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

            //wenn im dictionary "Header" ein value paar mit key "Authoriazation steht dann speicher ich den anderen wert in authToken
            if (request.Header.TryGetValue("Authorization", out var authToken))
            {
                const string prefix = "Bearer ";
                if (authToken.StartsWith(prefix))
                {
                    currentUser = _userManager.GetUserByAuthToken(authToken.Substring(prefix.Length));
                }
            }

            return currentUser;
        }
    }
}
