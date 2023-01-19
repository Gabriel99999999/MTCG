using MTCGServer.BLL.Exceptions;
using MTCGServer.BLL;
using MTCGServer.Core.Response;
using MTCGServer.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCGServer.Core.Routing;
using MTCGServer.Core.Request;

namespace MTCGServer.API.RouteCommands.Cards
{
    internal class GetDeckCommand : ICommand
    {
        private ICardManager _cardManager;
        private User _user;
        private bool _formatPlain;

        public GetDeckCommand(ICardManager cardManager, User user, Request request)
        {
            _cardManager = cardManager;
            _user = user;
            _formatPlain = GetFormatOutOfRequest(request);
        }

        private bool GetFormatOutOfRequest(Request request)
        {
            int index = request.ResourcePath.IndexOf("?format=plain");
            if (index > 0)
            {
                return true;
            }
            return false;
        }

        public Response Execute()
        {
            Response response = new Response();
            try
            {
                List<Card> stack = _cardManager.GetDeck(_user);
                if (stack.Any())
                {
                    response.StatusCode = StatusCode.Ok;
                    if (_formatPlain)
                    {
                        string responseString = string.Empty;
                        foreach (Card card in stack)
                        {
                            responseString += $"Die Karte: {card.Id} ist ein {card.Name} mit dem Damage Wert: {card.Damage}\n";
                        }
                        responseString.Remove(responseString.Length - 1);
                        response.Payload = responseString;
                    }
                    else
                    {
                        response.Payload = JsonConvert.SerializeObject(stack);
                    }
                }
                else
                {
                    response.StatusCode = StatusCode.NoContent;
                }
            }
            catch (Exception ex)
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
