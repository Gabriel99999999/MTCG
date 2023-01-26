using MTCGServer.API.RouteCommands.Cards;
using MTCGServer.BLL;
using MTCGServer.Core.Response;
using MTCGServer.Core.Routing;
using MTCGServer.DAL.Game;
using MTCGServer.Models;
using Newtonsoft.Json;

namespace MTCGServer.API.RouteCommands.Game
{
    public class GetStatsCommand : AuthenticatedRouteCommand
    {
        private IGameManager _gameManager;

        public GetStatsCommand(IGameManager gameManager, User user) : base(user)
        {
            _gameManager = gameManager;
        }

        public override Response Execute()
        {
            Response response= new Response();

            response.StatusCode = StatusCode.Ok;
            response.Payload = JsonConvert.SerializeObject(_user.ScoreboardData, Formatting.Indented); 
            //_gameManager.GetIndividuelScoreboardData(_user);

            return response;
        }
    }
}