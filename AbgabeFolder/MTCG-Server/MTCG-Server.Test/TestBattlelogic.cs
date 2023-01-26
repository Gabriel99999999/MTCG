using Moq;
using MTCGServer.BLL;
using MTCGServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace MTCGServer.Test
{
    public class TestBattlelogic
    {
        //Test the function Special Rules:
        [Test]
        public void TestReturnTrueAndChangementInDeckOfSpecialRuleWithGoblinsVSDragon()
        {
            Credentials exampleCredentials = new Credentials("username", "password");
            UserData fakeUserdata = new UserData();
            ScoreboardData scoreboardData = new ScoreboardData();
            User user1 = new User(exampleCredentials, 20, fakeUserdata, "token", scoreboardData);
            User user2 = new User(exampleCredentials, 20, fakeUserdata, "token", scoreboardData);
            Battlefield battlefield = new Battlefield(user1, user2);

            MonsterCard Wgoblin = new MonsterCard(new Guid(), "WaterGoblin", 10);
            MonsterCard Fgoblin = new MonsterCard(new Guid(), "FireGoblin", 10);
            MonsterCard Rgoblin = new MonsterCard(new Guid(), "RegularGoblin", 10);
            MonsterCard dragon = new MonsterCard(new Guid(), "Dragon", 10);

            user1.Deck.Add(Wgoblin);
            user2.Deck.Add(dragon);

            Assert.That(battlefield.SpecialRule(Wgoblin, dragon), Is.True);
            Assert.That(user1.Deck.Count(), Is.EqualTo(0));
            Assert.That(user2.Deck.Count(), Is.EqualTo(2));
            Assert.That(battlefield.SpecialRule(Fgoblin, dragon), Is.True);
            Assert.That(battlefield.SpecialRule(Rgoblin, dragon), Is.True);
        }

        [Test]
        public void TestReturnTrueAndChangementInDeckOfSpecialRuleWithWizzardVsOrk()
        {
            Credentials exampleCredentials = new Credentials("username", "password");
            UserData fakeUserdata = new UserData();
            ScoreboardData scoreboardData = new ScoreboardData();
            User user1 = new User(exampleCredentials, 20, fakeUserdata, "token", scoreboardData);
            User user2 = new User(exampleCredentials, 20, fakeUserdata, "token", scoreboardData);
            Battlefield battlefield = new Battlefield(user1, user2);

            MonsterCard wizzard = new MonsterCard(new Guid(), "Wizzard", 10);
            MonsterCard ork = new MonsterCard(new Guid(), "Ork", 10);

            user1.Deck.Add(wizzard);
            user2.Deck.Add(ork);

            Assert.That(battlefield.SpecialRule(wizzard, ork), Is.True);
            //check if amout of cards in deck has changed correctly
            Assert.That(user1.Deck.Count(), Is.EqualTo(2));
            Assert.That(user2.Deck.Count(), Is.EqualTo(0));
        }

        [Test]
        public void TestReturnTrueAndChangementInDeckOfSpecialRuleWithWaterSpellVsKnight()
        {
            Credentials exampleCredentials = new Credentials("username", "password");
            UserData fakeUserdata = new UserData();
            ScoreboardData scoreboardData = new ScoreboardData();
            User user1 = new User(exampleCredentials, 20, fakeUserdata, "token", scoreboardData);
            User user2 = new User(exampleCredentials, 20, fakeUserdata, "token", scoreboardData);
            Battlefield battlefield = new Battlefield(user1, user2);

            MonsterCard knight = new MonsterCard(new Guid(), "Knight", 10);
            SpellCard waterSpell = new SpellCard(new Guid(), "WaterSpell", 10);

            user1.Deck.Add(knight);
            user2.Deck.Add(waterSpell);

            Assert.That(battlefield.SpecialRule(knight, waterSpell), Is.True);
            //check if amout of cards in deck has changed correctly
            Assert.That(user1.Deck.Count(), Is.EqualTo(0));
            Assert.That(user2.Deck.Count(), Is.EqualTo(2));
        }

        [Test]
        public void TestReturnTrueAndChangementInDeckOfSpecialRuleWithKrakenVsSpells()
        {
            Credentials exampleCredentials = new Credentials("username", "password");
            UserData fakeUserdata = new UserData();
            ScoreboardData scoreboardData = new ScoreboardData();
            User user1 = new User(exampleCredentials, 20, fakeUserdata, "token", scoreboardData);
            User user2 = new User(exampleCredentials, 20, fakeUserdata, "token", scoreboardData);
            Battlefield battlefield = new Battlefield(user1, user2);

            MonsterCard kraken = new MonsterCard(new Guid(), "Kraken", 10);
            SpellCard waterSpell = new SpellCard(new Guid(), "WaterSpell", 10);
            SpellCard fireSpell = new SpellCard(new Guid(), "FireSpell", 10);
            SpellCard regularSpell = new SpellCard(new Guid(), "RegularSpell", 10);

            user1.Deck.Add(kraken);
            user2.Deck.Add(waterSpell);

            //test if special rule returns true
            Assert.That(battlefield.SpecialRule(kraken, waterSpell), Is.True);
            //check if amout of cards in deck has changed correctly
            Assert.That(user1.Deck.Count(), Is.EqualTo(2));
            Assert.That(user2.Deck.Count(), Is.EqualTo(0));

            //test if special rule returns true
            Assert.That(battlefield.SpecialRule(kraken, fireSpell), Is.True);
            Assert.That(battlefield.SpecialRule(kraken, regularSpell), Is.True);
            Assert.That(battlefield.SpecialRule(waterSpell, kraken), Is.True);
            Assert.That(battlefield.SpecialRule(fireSpell, kraken), Is.True);
            Assert.That(battlefield.SpecialRule(regularSpell, kraken), Is.True);

        }

        [Test]
        public void TestReturnTrueAndChangementInDeckOfSpecialRuleWithFireElvesVsDragons()
        {
            Credentials exampleCredentials = new Credentials("username", "password");
            UserData fakeUserdata = new UserData();
            ScoreboardData scoreboardData = new ScoreboardData();
            User user1 = new User(exampleCredentials, 20, fakeUserdata, "token", scoreboardData);
            User user2 = new User(exampleCredentials, 20, fakeUserdata, "token", scoreboardData);
            Battlefield battlefield = new Battlefield(user1, user2);

            MonsterCard fireElve = new MonsterCard(new Guid(), "FireElf", 10);
            MonsterCard dragon = new MonsterCard(new Guid(), "Dragon", 10);

            user1.Deck.Add(fireElve);
            user2.Deck.Add(dragon);

            Assert.That(battlefield.SpecialRule(fireElve, dragon), Is.True);
            Assert.That(user1.Deck.Count(), Is.EqualTo(2));
            Assert.That(user2.Deck.Count(), Is.EqualTo(0));
        }

        [Test]
        public void TestReturnFalseAndChangementInDeckOfSpecialRuleWithFireElvesVsDragons()
        {
            Credentials exampleCredentials = new Credentials("username", "password");
            UserData fakeUserdata = new UserData();
            ScoreboardData scoreboardData = new ScoreboardData();
            User user1 = new User(exampleCredentials, 20, fakeUserdata, "token", scoreboardData);
            User user2 = new User(exampleCredentials, 20, fakeUserdata, "token", scoreboardData);
            Battlefield battlefield = new Battlefield(user1, user2);

            MonsterCard kraken = new MonsterCard(new Guid(), "Kraken", 10);
            MonsterCard dragon = new MonsterCard(new Guid(), "Dragon", 10);

            user1.Deck.Add(kraken);
            user2.Deck.Add(dragon);

            Assert.That(battlefield.SpecialRule(kraken, dragon), Is.False);
            Assert.That(user1.Deck.Count(), Is.EqualTo(1));
            Assert.That(user2.Deck.Count(), Is.EqualTo(1));
        }

        //Tests for the function StartFight with a Pure Moster Fight
        [Test]
        public void TestStartFightWithPureMonsterFightUser1Wins()
        {
            Credentials exampleCredentials = new Credentials("username", "password");
            UserData fakeUserdata = new UserData();
            ScoreboardData scoreboardData = new ScoreboardData();
            User user1 = new User(exampleCredentials, 20, fakeUserdata, "token", scoreboardData);
            User user2 = new User(exampleCredentials, 20, fakeUserdata, "token", scoreboardData);
            Battlefield battlefield = new Battlefield(user1, user2);

            MonsterCard kraken = new MonsterCard(new Guid(), "Kraken", 20);
            MonsterCard dragon = new MonsterCard(new Guid(), "Dragon", 10);

            user1.Deck.Add(kraken);
            user2.Deck.Add(dragon);

            bool updateUser = true;


            battlefield.StartFight(ref updateUser);
            Assert.That(updateUser == true);
            Assert.That(user1.Deck.Count(), Is.EqualTo(2));
            Assert.That(user2.Deck.Count(), Is.EqualTo(0));
        }

        //Tests for the function StartFight with a Pure Moster Fight
        [Test]
        public void TestStartFightElementEffectedFightUser1Wins()
        {
            Credentials exampleCredentials = new Credentials("username", "password");
            UserData fakeUserdata = new UserData();
            ScoreboardData scoreboardData = new ScoreboardData();
            User user1 = new User(exampleCredentials, 20, fakeUserdata, "token", scoreboardData);
            User user2 = new User(exampleCredentials, 20, fakeUserdata, "token", scoreboardData);
            Battlefield battlefield = new Battlefield(user1, user2);

            SpellCard waterSpell = new SpellCard(new Guid(), "WaterSpell", 20);
            MonsterCard dragon = new MonsterCard(new Guid(), "Dragon", 10);

            user1.Deck.Add(waterSpell);
            user2.Deck.Add(dragon);

            bool updateUser = true;


            battlefield.StartFight(ref updateUser);
            Assert.That(updateUser == true);
            Assert.That(user1.Deck.Count(), Is.EqualTo(2));
            Assert.That(user2.Deck.Count(), Is.EqualTo(0));
        }

        [Test]
        public void TestStartFightElementEffectedFightUser2WinsAndCheckBattleLog()
        {
            Credentials exampleCredentials1 = new Credentials("username1", "password");
            Credentials exampleCredentials2 = new Credentials("username2", "password");
            UserData fakeUserdata = new UserData();
            ScoreboardData scoreboardData = new ScoreboardData();
            User user1 = new User(exampleCredentials1, 20, fakeUserdata, "token", scoreboardData);
            User user2 = new User(exampleCredentials2, 20, fakeUserdata, "token", scoreboardData);
            Battlefield battlefield = new Battlefield(user1, user2);

            MonsterCard knight = new MonsterCard(new Guid(), "Knight", 10);
            SpellCard fireSpell = new SpellCard(new Guid(), "FireSpell", 20);


            bool updateUser = true;
            string expectedBattleLog = "============== Round 1 ==============\n\nPlayer2 (username2) win with: FireSpell Damage: 20 => calculated Damage: 40 against\nPlayer1 (username1):Knight Damage: 10 => calculated Damage: 5\n\n----------- username2 (player2) won the battle against username1 (player1) -----------\n";
            user1.Deck.Add(knight);
            user2.Deck.Add(fireSpell);

            Assert.That(battlefield.StartFight(ref updateUser), Is.EqualTo(expectedBattleLog));
            Assert.That(updateUser == true);
            Assert.That(user1.Deck.Count(), Is.EqualTo(0));
            Assert.That(user2.Deck.Count(), Is.EqualTo(2));
        }

        [Test]
        public void TestStartFightElementEffectedFightWithDraw()
        {
            Credentials exampleCredentials = new Credentials("username", "password");
            UserData fakeUserdata = new UserData();
            ScoreboardData scoreboardData = new ScoreboardData();
            User user1 = new User(exampleCredentials, 20, fakeUserdata, "token", scoreboardData);
            User user2 = new User(exampleCredentials, 20, fakeUserdata, "token", scoreboardData);
            Battlefield battlefield = new Battlefield(user1, user2);

            MonsterCard dragon = new MonsterCard(new Guid(), "Dragon", 20);
            SpellCard fireSpell = new SpellCard(new Guid(), "FireSpell", 20);


            user1.Deck.Add(dragon);
            user2.Deck.Add(fireSpell);

            bool updateUser = true;

            battlefield.StartFight(ref updateUser);
            Assert.That(updateUser, Is.EqualTo(false));
            Assert.That(user1.Deck.Count(), Is.EqualTo(1));
            Assert.That(user2.Deck.Count(), Is.EqualTo(1));
        }
    }
}
