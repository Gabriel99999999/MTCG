using MTCGServer.BLL;
using MTCGServer.BLL.Exceptions;
using MTCGServer.Core.Request;
using MTCGServer.Core.Response;
using MTCGServer.Core.Routing;
using MTCGServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGServer.API.RouteCommands.Packages
{
    internal class CreatePackageCommand : ICommand
    {
        private readonly IPackageManager _packageManager;
        private readonly Package _package;
        private readonly User _user;


        public CreatePackageCommand(IPackageManager packageManager, List<Card> package, User user)
        {
            _packageManager = packageManager;
            _package = new Package(package);
            _user = user;
        }

        public Response Execute()
        {
            Response response= new Response();
            //if user != admin but token was set (false token)
            try
            {
                if (_user.Credentials.Username != "admin")
                {
                    response.StatusCode = StatusCode.Forbidden;
                }
                else
                {
                    if (_packageManager.AddPackage(_package))
                    {
                        response.StatusCode = StatusCode.Created;
                    }
                    else
                    {
                        response.StatusCode = StatusCode.Conflict;
                    }
                }
            }
            catch(Exception ex)
            {
                if(ex is DuplicateDataException)
                {
                    response.StatusCode = StatusCode.Conflict;
                }
                if(ex is DataAccessException) 
                { 
                    response.StatusCode = StatusCode.InternalServerError;
                }
            }
            return response;
        }
    }
}
