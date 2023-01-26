using Moq;
using MTCGServer.API.RouteCommands;
using MTCGServer.BLL;
using MTCGServer.Core.Request;
using MTCGServer.DAL.Users;
using MTCGServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGServer.Test
{
    internal class IdentityProviderTest
    {
        [Test]
        public void TestIdentityProvider()
        {
            var userDb = new MemoryUserDao();
            UserManager userManager = new UserManager(userDb);
            IdentityProvider identityProvider = new IdentityProvider(userManager);

            //manipulate the MemoryDatabase
            Credentials credentials = new Credentials("test", "password");
            userManager.RegisterUser(credentials);
            userManager.LoginUser(credentials);

            //create a fake Request
            Request request = new Request();
            request.Header.Add("Authorization","Bearer test-mtcgToken");

            User? result = identityProvider.GetIdentityForRequest(request);
            
            Assert.IsNotNull(result);
            Assert.That(result.Credentials, Is.EqualTo(credentials));

        }
    }
}
