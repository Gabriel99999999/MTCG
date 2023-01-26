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
    public class AcquirePackageCommand : AuthenticatedRouteCommand
    {
        private IPackageManager _packageManager;

        public AcquirePackageCommand(IPackageManager packageManager, User user) : base(user) 
        {
            _packageManager = packageManager;
        }

        public override Response Execute()
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
