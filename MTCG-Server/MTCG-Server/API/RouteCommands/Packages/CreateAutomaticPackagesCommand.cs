using MTCGServer.BLL;
using MTCGServer.BLL.Exceptions;
using MTCGServer.Core.Response;
using MTCGServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGServer.API.RouteCommands.Packages
{
    public class CreateAutomaticPackagesCommand : AuthenticatedRouteCommand
    {
        IPackageManager _packageManager;
        int _amount;
        bool _parsingWorked;
        public CreateAutomaticPackagesCommand(IPackageManager packageManager, User user, string zahl) : base(user)
        {
            _packageManager = packageManager;
            try
            {
                Int32.TryParse(zahl, out _amount);
                _parsingWorked = true;
            }
            catch (FormatException)
            {
                _parsingWorked = false;
                _amount = 0;
            }
        }
        public override Response Execute()
        {
            Response response= new Response();

            //create Packages:
            try
            {
                if (_parsingWorked)
                {
                    //Payload was a number
                    if (_amount > 10 || _amount <= 0)
                    { 
                        response.StatusCode = StatusCode.BadRequest;
                        response.Payload = "you entered a too large/small number of packages MAX=10 MIN=1";
                        return response;
                    }
                    for (int count = 1; count <= _amount; count++)
                    {
                        List<Card> cards = generatePackage();
                        Package package = new Package(cards);

                        if (_user.Credentials.Username != "admin")
                        {
                            response.StatusCode = StatusCode.Forbidden;
                        }
                        else
                        {
                            try
                            {
                                if (_packageManager.AddPackage(package))
                                {
                                    response.StatusCode = StatusCode.Created;
                                }
                                else
                                {
                                    response.StatusCode = StatusCode.Conflict;
                                }
                            }
                            catch (Exception ex)
                            {
                                if (ex is DuplicateDataException)
                                {
                                    //create a new package for the one which did not work
                                    --_amount;
                                }
                                else
                                {
                                    throw;
                                }
                            }

                        }
                    }
                }
                else
                {
                    response.StatusCode = StatusCode.BadRequest;
                    response.Payload = "you have to enter a number";
                }
            }
            catch (Exception ex)
            {
                if (ex is DataAccessException)
                {
                    response.StatusCode = StatusCode.InternalServerError;
                }
            }
            return response;
        }

        private List<Card> generatePackage()
        {
            List<Card> cards = new List<Card>();
            Random random1 = new Random();
            Random random2 = new Random();
            for (int count = 1; count <= 4; count++)
            {
                string[] possibleNames = new string[17] { "WaterGoblin", "FireGoblin", "RegularGoblin", "WaterTroll", "FireTroll", "RegularTroll", "WaterElf", "FireElf", "RegularElf", "WaterSpell", "FireSpell", "RegularSpell", "Knight", "Dragon", "Ork", "Kraken", "Wizzard" };
                int indexOfName = random1.Next(1, possibleNames.Length);
                string name = possibleNames[indexOfName];
                decimal damage = (decimal)random2.Next(1, 100);

                Card card = new Card(Guid.NewGuid(), name, damage);
                cards.Add(card);
            }
            return cards; 
        }
    }
}
