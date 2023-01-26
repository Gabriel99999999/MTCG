using Moq;
using MTCGServer.API.RouteCommands.Packages;
using MTCGServer.BLL.Exceptions;
using MTCGServer.BLL;
using MTCGServer.Core.Response;
using MTCGServer.DAL.Cards;
using MTCGServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCGServer.API.RouteCommands.Cards;

namespace MTCGServer.Test
{
    public class TestCardDao
    {
        [Test]
        public void TestConfigureDeckCommandWhenConfigureDeckReturnsTrue()
        {
            var mockCardManager = new Mock<ICardManager>();
            User player1 = new User(new Credentials("martin", "password"));
            List<Guid> guids = new List<Guid>();
            guids.Add(Guid.NewGuid());
            guids.Add(Guid.NewGuid());
            guids.Add(Guid.NewGuid());
            guids.Add(Guid.NewGuid());
            ConfigureDeckCommand configureDeckCommand = new ConfigureDeckCommand(mockCardManager.Object, player1, guids);
            
            mockCardManager
                    .Setup(x => x.ConfigureDeck(player1, guids))
                    .Returns(true);

            Response result = configureDeckCommand.Execute();

            Assert.That(result.StatusCode, Is.EqualTo(StatusCode.Ok));
            Assert.That(result.Payload, Is.EqualTo(null));
        }

        [Test]
        public void TestConfigureDeckCommandWhenConfigureDeckReturnsFalse()
        {
            var mockCardManager = new Mock<ICardManager>();
            User player1 = new User(new Credentials("martin", "password"));
            List<Guid> guids = new List<Guid>();
            guids.Add(Guid.NewGuid());
            guids.Add(Guid.NewGuid());
            guids.Add(Guid.NewGuid());
            guids.Add(Guid.NewGuid());
            ConfigureDeckCommand configureDeckCommand = new ConfigureDeckCommand(mockCardManager.Object, player1, guids);

            mockCardManager
                    .Setup(x => x.ConfigureDeck(player1, guids))
                    .Returns(false);

            Response result = configureDeckCommand.Execute();

            Assert.That(result.StatusCode, Is.EqualTo(StatusCode.Forbidden));
            Assert.That(result.Payload, Is.EqualTo(null));
        }
    }
}
