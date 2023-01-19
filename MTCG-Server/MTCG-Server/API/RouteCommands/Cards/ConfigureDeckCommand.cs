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
    internal class ConfigureDeckCommand : ICommand
    {
        private ICardManager _cardManager;
        private User _user;
        private List<Guid> _guids;

        public ConfigureDeckCommand(ICardManager cardManager, User user, List<Guid> guids)
        {
            _cardManager = cardManager;
            _user = user;
            _guids = guids;
        }

        public Response Execute()
        {
            Response response = new Response();
            if(_guids.Count != 4) 
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