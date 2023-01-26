using Moq;
using MTCGServer.API.RouteCommands.Packages;
using MTCGServer.API.RouteCommands.Trading;
using MTCGServer.BLL;
using MTCGServer.BLL.Exceptions;
using MTCGServer.Core.Response;
using MTCGServer.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MTCGServer.Test
{
    internal class TestPackagesCommands
    {
        //Tests about AquirePackageCommand
        [Test]
        public void TestAquirePackageCommandWhenBuyPackageReturnsAList()
        {
            var mockPackageManager = new Mock<IPackageManager>();
            User player1 = new User(new Credentials("martin", "password"));
            AcquirePackageCommand createCommand = new AcquirePackageCommand(mockPackageManager.Object, player1);
            List<Card> package = new List<Card>();
            Card card1 = new Card(new Guid(), "WaterGoblin", 30);
            Card card2 = new Card(new Guid(), "Knight", 50);
            package.Add(card1); 
            package.Add(card2);

            mockPackageManager
                    .Setup(x => x.BuyPackage(player1))
                    .Returns(package);

            Response result = createCommand.Execute();

            Assert.That(result.StatusCode, Is.EqualTo(StatusCode.Ok));
            Assert.That(result.Payload, Is.EqualTo(JsonConvert.SerializeObject(package, Formatting.Indented)));
        }
        [Test]
        public void TestAquirePackageCommandWhenBuyPackageReturnsNull()
        {
            var mockPackageManager = new Mock<IPackageManager>();
            User player1 = new User(new Credentials("martin", "password"));
            AcquirePackageCommand createCommand = new AcquirePackageCommand(mockPackageManager.Object, player1);
            List<Card>? package = null;

            mockPackageManager
                    .Setup(x => x.BuyPackage(player1))
                    .Returns(package);

            Response result = createCommand.Execute();

            Assert.That(result.StatusCode, Is.EqualTo(StatusCode.NotFound));
            Assert.That(result.Payload, Is.EqualTo(null));
        }

        [Test]
        public void TestAquirePackageCommandWhenBuyPackageThrowsTooLessMoneyException()
        {
            var mockPackageManager = new Mock<IPackageManager>();
            User player1 = new User(new Credentials("martin", "password"));
            AcquirePackageCommand createCommand = new AcquirePackageCommand(mockPackageManager.Object, player1);

            mockPackageManager
                    .Setup(x => x.BuyPackage(player1))
                    .Throws(new TooLessMoneyException());

            Response result = createCommand.Execute();

            Assert.That(result.StatusCode, Is.EqualTo(StatusCode.Forbidden));
            Assert.That(result.Payload, Is.EqualTo(null));
        }
    }
}
