using MTCGServer.BLL;
using MTCGServer.BLL.Exceptions;
using MTCGServer.Core.Response;
using MTCGServer.Core.Routing;
using MTCGServer.Models;

namespace MTCGServer.API.RouteCommands.Game
{
    public class EnterLobyCommand : AuthenticatedRouteCommand
    {
        private readonly IGameManager _gameManager;
        private readonly ICardManager _cardManager;
        private Lobby _lobby;

        public EnterLobyCommand(IGameManager gameManager, ICardManager cardManager, User user, Lobby lobby) : base(user)
        {
            _gameManager = gameManager;
            _cardManager = cardManager;
            _lobby = lobby; 
        }

        public override Response Execute()
        {
            Response response = new Response();
            
            
            try
            {
                _user.Deck = _cardManager.GetDeck(_user);
                if(_user.Deck.Count() != 4)
                {
                    throw new DeckNotCorrectConfiguredException();
                }
                bool updateUser = true; //if scoreboard data has to get updated
                bool worksFine = true;
                string battleLog = _lobby.EnterLobby(_user, ref updateUser);

                if (updateUser)
                {
                    worksFine = _gameManager.UpdateElo(_user);
                }

                if (worksFine)
                {
                    response.StatusCode = StatusCode.Ok;
                    response.Payload = battleLog;
                }
                else
                {   
                    //updating Users Scoreboard Data fails
                    response.StatusCode = StatusCode.InternalServerError;
                }
            }
            catch (Exception ex)
            {
                if (ex is DataAccessException) { response.StatusCode = StatusCode.InternalServerError; }
                else if(ex is DeckNotCorrectConfiguredException) { response.StatusCode= StatusCode.Conflict; }
                else { response.StatusCode = StatusCode.InternalServerError;}
            }


            return response;
        }
    }
}