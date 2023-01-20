using MTCGServer.API.RouteCommands.Packages;
using MTCGServer.BLL;
using MTCGServer.BLL.Exceptions;
using MTCGServer.Core.Response;
using MTCGServer.Core.Routing;
using MTCGServer.Models;
using Newtonsoft.Json;

namespace MTCGServer.API.RouteCommands.Cards
{
    internal class GetStackCommand : ICommand
    {
        private ICardManager _cardManager;
        private User _user;

        public GetStackCommand(ICardManager cardManager, User user)
        {
            _cardManager = cardManager;
            _user = user;
        }

        public Response Execute()
        {
            Response response= new Response();
            try
            {
                List<Card> stack = _cardManager.GetStack(_user);
                if (stack.Any())
                {
                    response.StatusCode = StatusCode.Ok;
                    response.Payload = JsonConvert.SerializeObject(stack, Formatting.Indented);
                }
                else
                {
                    response.StatusCode = StatusCode.NoContent;
                }
            }
            catch(Exception ex)
            {
                if (ex is DataAccessException)
                {
                    response.StatusCode = StatusCode.InternalServerError;
                }
                else
                {
                    response.StatusCode = StatusCode.NotImplemented;
                }
            }
            

            return response;
        }
    }
}