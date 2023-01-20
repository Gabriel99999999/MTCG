using MTCGServer.BLL;
using MTCGServer.BLL.Exceptions;
using MTCGServer.Core.Response;
using MTCGServer.Core.Routing;
using MTCGServer.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGServer.API.RouteCommands.Packages
{
    internal class AcquirePackageCommand : ICommand
    {
        private IPackageManager _packageManager;
        private User _user;

        public AcquirePackageCommand(IPackageManager packageManager, User user)
        {
            _packageManager = packageManager;
            _user = user;
        }

        public Response Execute()
        {
            Response response = new Response();
            try
            {
                List<Card>? package = _packageManager.BuyPackage(_user);
                if (package == null)
                {   
                    //no Card package Available
                    response.StatusCode = StatusCode.NotFound;
                }
                else
                {
                    response.StatusCode = StatusCode.Ok;
                    response.Payload = JsonConvert.SerializeObject(package, Formatting.Indented);
                }
            }
            catch(Exception ex) 
            { 
                if(ex is TooLessMoneyException)
                {
                    response.StatusCode = StatusCode.Forbidden;
                }
                else if(ex is DataAccessException)
                {
                    response.StatusCode = StatusCode.InternalServerError;
                }
                else
                {
                    Console.WriteLine(ex.Message);
                    throw new NotImplementedException();
                }
            }
            return response;
        }
    }
}
