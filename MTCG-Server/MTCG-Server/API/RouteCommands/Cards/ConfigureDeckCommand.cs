using MTCGServer.BLL;
using MTCGServer.BLL.Exceptions;
using MTCGServer.Core.Response;
using MTCGServer.Core.Routing;
using MTCGServer.DAL.Exceptions;
using MTCGServer.DAL;
using MTCGServer.Models;
using Npgsql;

namespace MTCGServer.API.RouteCommands.Cards
{
    public class ConfigureDeckCommand : AuthenticatedRouteCommand
    {
        private readonly ICardManager _cardManager;
        private readonly List<Guid> _guids;

        public ConfigureDeckCommand(ICardManager cardManager, User user, List<Guid> guids) : base(user)
        {
            _cardManager = cardManager;
            _guids = guids;
        }

        public override Response Execute()
        {
            Response response = new Response();
            if(_guids.Count != 4 || _guids.Distinct().Count() != _guids.Count) 
            { 
                response.StatusCode = StatusCode.BadRequest;
            }
            else
            {
                try
                {
                    bool worksFine = _cardManager.ConfigureDeck(_user, _guids);
                    if (worksFine)
                    {
                        response.StatusCode = StatusCode.Ok;
                    }
                    else
                    {
                        //at least one oft the provided cards does not belong to the user or is not available for example is part of a current trade
                        response.StatusCode = StatusCode.Forbidden;
                    }
                }
                catch (Exception ex)
                {
                    if (ex is UpdateFailsException) { response.StatusCode = StatusCode.InternalServerError; }
                    else if (ex is DataAccessException) { response.StatusCode = StatusCode.InternalServerError; }
                    else if (ex is DatabaseException) { response.StatusCode = StatusCode.InternalServerError; }
                    else 
                    {
                        Console.WriteLine(ex.Message);
                        throw new NotImplementedException();
                    }
                }
            }

            return response;
        }
    }
}