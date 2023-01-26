using Moq;
using MTCGServer.API.RouteCommands.Trading;
using MTCGServer.BLL;
using MTCGServer.BLL.Exceptions;
using MTCGServer.Core.Response;
using MTCGServer.DAL.Trading;
using MTCGServer.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MTCGServer.Test
{
    internal class TestTradingCommands
    {
     //Tests about CreateTradeCommand
        [Test]
        public void TestCreateTradeCommandWhenCreateTradeReturnsTrue()
        {
            var mockTradeManager = new Mock<ITradingManager>();
            Guid tradeId = Guid.NewGuid();
            Guid cardId = Guid.NewGuid();
            string type = "monster";
            decimal minDamage = 10;
            Trade trade = new Trade(tradeId, cardId, type, minDamage);
            User player1 = new User(new Credentials("martin", "password"));
            CreateTradeCommand createCommand = new CreateTradeCommand(mockTradeManager.Object, player1, trade);

            mockTradeManager
                    .Setup(x => x.CreateTrade(It.IsAny<Trade>(), It.IsAny<User>()))
                    .Returns(true);

            Response result = createCommand.Execute();

            Assert.That(result.StatusCode, Is.EqualTo(StatusCode.Created));
            Assert.That(result.Payload, Is.EqualTo(null));
        }

        [Test]
        public void TestCreateTradeCommandWhenCreateTradeReturnsFalse()
        {
            var mockTradeManager = new Mock<ITradingManager>();
            Guid tradeId = Guid.NewGuid();
            Guid cardId = Guid.NewGuid();
            string type = "monster";
            decimal minDamage = 10;
            Trade trade = new Trade(tradeId, cardId, type, minDamage);
            User player1 = new User(new Credentials("martin", "password"));
            CreateTradeCommand createCommand = new CreateTradeCommand(mockTradeManager.Object, player1, trade);

            mockTradeManager
                    .Setup(x => x.CreateTrade(It.IsAny<Trade>(), It.IsAny<User>()))
                    .Returns(false);

            Response result = createCommand.Execute();

            Assert.That(result.StatusCode, Is.EqualTo(StatusCode.Forbidden));
            Assert.That(result.Payload, Is.EqualTo(null));
        }

        [Test]
        public void TestCreateTradeCommandWhenCreateTradeThrowsDuplicateDataException()
        {
            var mockTradeManager = new Mock<ITradingManager>();
            Guid tradeId = Guid.NewGuid();
            Guid cardId = Guid.NewGuid();
            string type = "monster";
            decimal minDamage = 10;
            Trade trade = new Trade(tradeId, cardId, type, minDamage);
            User player1 = new User(new Credentials("martin", "password"));
            CreateTradeCommand createCommand = new CreateTradeCommand(mockTradeManager.Object, player1, trade);

            mockTradeManager
                    .Setup(x => x.CreateTrade(It.IsAny<Trade>(), It.IsAny<User>()))
                    .Throws( new DuplicateDataException());

            Response result = createCommand.Execute();

            Assert.That(result.StatusCode, Is.EqualTo(StatusCode.Conflict));
            Assert.That(result.Payload, Is.EqualTo(null));
        }

    //Tests about MakeTradeCommand
        [Test]
        public void TestMakeTradeCommandWhenMakeTradeReturnsTrue()
        {
            var mockTradeManager = new Mock<ITradingManager>();
            string tradeId = new Guid().ToString(); 
            string cardId = new Guid().ToString();  
            User player1 = new User(new Credentials("martin", "password"));
            MakeTradeCommand createCommand = new MakeTradeCommand(mockTradeManager.Object, player1, tradeId, cardId);

            mockTradeManager
                    .Setup(x => x.MakeTrade(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<User>()))
                    .Returns(true);

            Response result = createCommand.Execute();

            Assert.That(result.StatusCode, Is.EqualTo(StatusCode.Ok));
            Assert.That(result.Payload, Is.EqualTo(null));
        }

        [Test]
        public void TestMakeTradeCommandWhenMakeTradeReturnsFalse()
        {
            var mockTradeManager = new Mock<ITradingManager>();
            string tradeId = new Guid().ToString();
            string cardId = new Guid().ToString();
            User player1 = new User(new Credentials("martin", "password"));
            MakeTradeCommand createCommand = new MakeTradeCommand(mockTradeManager.Object, player1, tradeId, cardId);

            mockTradeManager
                    .Setup(x => x.MakeTrade(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<User>()))
                    .Returns(false);

            Response result = createCommand.Execute();

            Assert.That(result.StatusCode, Is.EqualTo(StatusCode.Forbidden));
            Assert.That(result.Payload, Is.EqualTo(null));
        }

        [Test]
        public void TestMakeTradeCommandWhenMakeTradeThrowsDataNotFoundException()
        {
            var mockTradeManager = new Mock<ITradingManager>();
            string tradeId = new Guid().ToString();
            string cardId = new Guid().ToString();
            User player1 = new User(new Credentials("martin", "password"));
            MakeTradeCommand createCommand = new MakeTradeCommand(mockTradeManager.Object, player1, tradeId, cardId);

            mockTradeManager
                    .Setup(x => x.MakeTrade(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<User>()))
                    .Throws(new DataNotFoundException());

            Response result = createCommand.Execute();

            Assert.That(result.StatusCode, Is.EqualTo(StatusCode.NotFound));
            Assert.That(result.Payload, Is.EqualTo(null));
        }

    //Tests about GetTradesCommand
        [Test]
        public void TestGetTradesCommandWhenGetTradesReturnListOfTrades()
        {
            var mockTradeManager = new Mock<ITradingManager>();
            User player1 = new User(new Credentials("martin", "password"));
            GetTradesCommand createCommand = new GetTradesCommand(mockTradeManager.Object, player1);
            List<Trade> expectedListOfTrades = new List<Trade>();
            Trade trade1 = new Trade(new Guid(), new Guid(), "monster", 10);
            Trade trade2 = new Trade(new Guid(), new Guid(), "monster", 10);
            expectedListOfTrades.Add(trade1);
            expectedListOfTrades.Add(trade2);

            mockTradeManager
                    .Setup(x => x.GetTrades())
                    .Returns(expectedListOfTrades);

            Response result = createCommand.Execute();

            Assert.That(result.StatusCode, Is.EqualTo(StatusCode.Ok));
            Assert.That(result.Payload, Is.EqualTo(JsonConvert.SerializeObject(expectedListOfTrades, Formatting.Indented)));
        }

        [Test]
        public void TestGetTradesCommandWhenGetTradesReturnEmptyList()
        {
            var mockTradeManager = new Mock<ITradingManager>();
            User player1 = new User(new Credentials("martin", "password"));
            GetTradesCommand createCommand = new GetTradesCommand(mockTradeManager.Object, player1);

            mockTradeManager
                    .Setup(x => x.GetTrades())
                    .Returns(new List<Trade>());

            Response result = createCommand.Execute();

            Assert.That(result.StatusCode, Is.EqualTo(StatusCode.NoContent));
            Assert.That(result.Payload, Is.EqualTo(null));
        }

     //Tests about DeleteExistingTrade Command
        [Test]
        public void TestDeleteExistingTradeCommandWhenDeleteTradeReturnsTrue()
        {
            var mockTradeManager = new Mock<ITradingManager>();
            Guid tradeId = new Guid();
            User player1 = new User(new Credentials("martin", "password"));
            DeleteExistingTradeCommand createCommand = new DeleteExistingTradeCommand(mockTradeManager.Object, player1, tradeId.ToString());

            mockTradeManager
                    .Setup(x => x.DeleteTrade(player1, tradeId))
                    .Returns(true);

            Response result = createCommand.Execute();

            Assert.That(result.StatusCode, Is.EqualTo(StatusCode.Ok));
            Assert.That(result.Payload, Is.EqualTo(null));
        }

        [Test]
        public void TestDeleteExistingTradeCommandWhenDeleteTradeReturnsFalse()
        {
            var mockTradeManager = new Mock<ITradingManager>();
            Guid tradeId = new Guid();
            User player1 = new User(new Credentials("martin", "password"));
            DeleteExistingTradeCommand createCommand = new DeleteExistingTradeCommand(mockTradeManager.Object, player1, tradeId.ToString());

            mockTradeManager
                    .Setup(x => x.DeleteTrade(player1, tradeId))
                    .Returns(false);

            Response result = createCommand.Execute();

            Assert.That(result.StatusCode, Is.EqualTo(StatusCode.Forbidden));
            Assert.That(result.Payload, Is.EqualTo(null));
        }

        [Test]
        public void TestDeleteExistingTradeCommandWhenDeleteTradeThrowsDataNotFoundException()
        {
            var mockTradeManager = new Mock<ITradingManager>();
            Guid tradeId = new Guid();
            User player1 = new User(new Credentials("martin", "password"));
            DeleteExistingTradeCommand createCommand = new DeleteExistingTradeCommand(mockTradeManager.Object, player1, tradeId.ToString());

            mockTradeManager
                    .Setup(x => x.DeleteTrade(player1, tradeId))
                    .Throws(new DataNotFoundException());

            Response result = createCommand.Execute();

            Assert.That(result.StatusCode, Is.EqualTo(StatusCode.NotFound));
            Assert.That(result.Payload, Is.EqualTo(null));
        }

        /*
        [Test]
        public void TestCreateTradeCommandSuccesfully2()
        {
            var mockTradeDao = new Mock<ITradingDao>();
            var tradeManager = new TradingManager(mockTradeDao.Object);
            Guid tradeId = Guid.NewGuid();
            Guid cardId = Guid.NewGuid();
            string type = "monster";
            decimal minDamage = 10;
            Trade trade = new Trade(tradeId, cardId, type, minDamage);
            User player1 = new User(new Credentials("martin", "password"));

            mockTradeDao
                    .Setup(x => x.CreateTrade(It.IsAny<Trade>(), It.IsAny<User>()))
                    .Returns(true);

            bool result = tradeManager.CreateTrade(trade, player1);
            Assert.IsTrue(result);
        }

        public void TestTradingManagerSuccesfully3()
        {
            var mockTradeDao = new Mock<ITradingDao>();
            var tradeManager = new TradingManager(mockTradeDao.Object);
            Guid tradeId = Guid.NewGuid();
            Guid cardId = Guid.NewGuid();
            string type = "monster";
            decimal minDamage = 10;
            Trade trade = new Trade(tradeId, cardId, type, minDamage);
            User player1 = new User(new Credentials("martin", "password"));

            mockTradeDao
                    .Setup(x => x.CreateTrade(It.IsAny<Trade>(), It.IsAny<User>()))
                    .Returns(false);

            bool result = tradeManager.CreateTrade(trade, player1);
            Assert.IsFalse(result);
        }*/
    }
}
