using MTCGServer.BLL;
using MTCGServer.BLL.Exceptions;
using MTCGServer.Core.Response;
using MTCGServer.Core.Routing;
using MTCGServer.Models;
using System.Diagnostics;

namespace MTCGServer.API.RouteCommands.Trading
{
    public class CreateTradeCommand : AuthenticatedRouteCommand
    {
        private Trade _trade;
        private ITradingManager _tradingManager;

        public CreateTradeCommand(ITradingManager tradeManager, User user, Trade trade) : base(user) 
        {
            _trade = trade;
            _tradingManager = tradeManager;
        }

        public override Response Execute()
        {
            Response response= new Response();
            try
            {
                bool successfullCreated = _tradingManager.CreateTrade(_trade, _user);
                if (successfullCreated)
                {
                    response.StatusCode = StatusCode.Created;
                }
                else
                {
                    response.StatusCode = StatusCode.Forbidden; //The deal contains a card that is not owned by the user or locked in the deck.
                }
            }
            catch(Exception ex) 
            {
                if(ex is DuplicateDataException)
                {
                    response.StatusCode = StatusCode.Conflict;
                }
                else if(ex is DataAccessException)
                {
                    response.StatusCode = StatusCode.InternalServerError;
                }
                else
                {
                    response.StatusCode = StatusCode.InternalServerError;
                }
            }
            return response;
        }
    }
}