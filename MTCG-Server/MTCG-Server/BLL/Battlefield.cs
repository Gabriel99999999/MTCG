using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MTCGServer.Models;

namespace MTCGServer.BLL
{
    public class Battlefield
    {
        private User _player1;
        private User _player2;
        private string _battleLock;

        public Battlefield(User player1, User player2)
        {
            _player1 = player1;
            _player2 = player2;
            _battleLock = "";
        }

        public string StartFight(ref bool updateUser)
        {
            int round = 1;
            Card card1, card2;
            Random random = new Random();
            int amount1 = 0;
            int amount2 = 0;
            
            while (_player1.Deck.Count > 0 && _player2.Deck.Count > 0 && round <= 100)
            {
                _battleLock += $"============== Round {round} ==============\n\n";
                amount1 = _player1.Deck.Count;
                amount2 = _player2.Deck.Count;
                card1 = _player1.Deck[random.Next(amount1)];
                card2 = _player2.Deck[random.Next(amount2)];
                if (!SpecialRule(card1, card2))
                {
                    if (CompareCurrentFighter(card1, card2))
                    {
                        //fight influenced by the elements
                        NotElementInfluencedF(card1, card2);
                    }
                    else
                    {
                        //fight which is influenced by the elements
                        ElementInfluencedF(card1, card2);
                    }
                }

                round++;
            }

            if (_player1.Deck.Count is 0)
            {
                _battleLock += $"----------- {_player2.Credentials.Username} (player2) won the battle against {_player1.Credentials.Username} (player1) -----------\n";
                UpdateWinner(_player2);
                UpdateLooser(_player1);


            }

            else if (_player2.Deck.Count is 0)
            {
                _battleLock += $"----------- {_player1.Credentials.Username} (player1) won the battle against {_player2.Credentials.Username} (player2) -----------\n";
                UpdateWinner(_player1);
                UpdateLooser(_player2);
            }
            else
            {
                _battleLock += $"-------------------DRAW-------------------\n";
                updateUser = false;
            }

            return _battleLock;
        }
        public bool CompareCurrentFighter(Card card1, Card card2)
        {
            if (card1 is MonsterCard && card2 is MonsterCard || card1.Element == card2.Element)
                return true;

            return false;
        }

        //returnt true falls eine Special Rule den Fight entscheidet
        public bool SpecialRule(Card card1, Card card2)
        {
            CardType[] goblins = { CardType.WaterGoblin, CardType.FireGoblin, CardType.RegularGoblin };
            //speichert winningCard
            Card? winCard = null;

            CardType[] spells = { CardType.WaterSpell, CardType.FireSpell, CardType.RegularSpell };

            //Bedigungen wo card1 gewinnt
            //Dragon vs Goblin || Wizzard vs Ork || WaterSpell vs Knight || Kraken vs spells || FireElve vs Dragon
            if (card1.NameEnum is CardType.Dragon && Array.Exists(goblins, goblin => goblin == card2.NameEnum) ||
                 card1.NameEnum is CardType.Wizzard && card2.NameEnum is CardType.Ork ||
                 card1.NameEnum is CardType.WaterSpell && card2.NameEnum is CardType.Knight ||
                 card1.NameEnum is CardType.Kraken && Array.Exists(spells, x => x == card2.NameEnum) ||
                 card1.NameEnum is CardType.FireElf && card2.NameEnum is CardType.Dragon)
            {
                winCard = card1;
            }
            //Bedigungen wo card2 gewinnt
            //Goblin vs Dragon || Ork vs Wizzard || Knight vs WaterSpell || spells vs Kraken ||Dragon vs FireElve
            else if (Array.Exists(goblins, goblin => goblin == card1.NameEnum) && card2.NameEnum is CardType.Dragon ||
                      card1.NameEnum is CardType.Ork && card2.NameEnum is CardType.Wizzard ||
                      card1.NameEnum is CardType.Knight && card2.NameEnum is CardType.WaterSpell ||
                      Array.Exists(spells, spell => spell == card1.NameEnum) && card2.NameEnum is CardType.Kraken ||
                      card1.NameEnum is CardType.Dragon && card2.NameEnum is CardType.FireElf)
            {
                winCard = card2;
            }

            if (winCard is not null)
            {
                if (winCard == card1)
                {
                    _battleLock += $"Player1 ({_player1.Credentials.Username}) win with: {card1.Name} Damage: {card1.Damage} against\nPlayer2 ({_player2.Credentials.Username}) with: {card2.Name} Damage: {card2.Damage}\n\n";
                    _player1.Deck.Add(card2);
                    _player2.Deck.Remove(card2);
                }
                else
                {
                    _battleLock += $"Player2 ({_player2.Credentials.Username}) win with: {card2.Name} Damage: {card2.Damage} against\nPlayer1 ({_player1.Credentials.Username}) with: {card1.Name} Damage: {card1.Damage}\n\n";
                    _player2.Deck.Add(card1);
                    _player1.Deck.Remove(card1);
                }
                return true;
            }

            return false;
        }

        public void NotElementInfluencedF(Card card1, Card card2)
        {
            if (card1.Damage == card2.Damage)
            {
                _battleLock += $"........................................Draw because:........................................\nPlayer1 ({_player1.Credentials.Username}) had: {card1.Name} Damage: {card1.Damage} and\nPlayer2 ({_player2.Credentials.Username}) had {card2.Name} Damage: {card2.Damage}\n\n";
            }
            else if (card1.Damage > card2.Damage)
            {
                _battleLock += $"Player1 ({_player1.Credentials.Username}) win with: {card1.Name} Damage: {card1.Damage} against\nPlayer2 ({_player2.Credentials.Username}) with: {card2.Name} Damage: {card2.Damage}\n\n";
                _player1.Deck.Add(card2);
                _player2.Deck.Remove(card2);
            }
            else
            {
                _battleLock += $"Player2 ({_player2.Credentials.Username}) win with: {card2.Name} Damage: {card2.Damage} against\nPlayer1 ({_player1.Credentials.Username}) with: {card1.Name} Damage: {card1.Damage}\n\n";
                _player2.Deck.Add(card1);
                _player1.Deck.Remove(card1);
            }
        }

        public void ElementInfluencedF(Card card1, Card card2)
        {
            decimal damage1 = card1.Damage;
            decimal damage2 = card2.Damage;
            //Bedingungen die card1 Damage verdoppeln und card2 Damage halbiert
            //Water vs Fire || Fire vs Normal || Normal vs Water
            if (card1.Element is ElementType.Water && card2.Element is ElementType.Fire ||
                card1.Element is ElementType.Fire && card2.Element is ElementType.Normal ||
                card1.Element is ElementType.Normal && card2.Element is ElementType.Water)
            {
                damage1 = damage1 * 2;
                damage2 = damage2 / 2;
            }
            //Bedingungen die card2 Damage verdoppeln und card1 Damage halbiert
            else
            {
                damage1 = damage1 / 2;
                damage2 = damage2 * 2;
            }

            if (card1.Damage == card2.Damage)
            {
                _battleLock += $"........................................Draw because:........................................\nPlayer1 ({_player1.Credentials.Username}) had: {card1.Name} Damage: {card1.Damage} => calculated Damage: {damage1} and\nPlayer2 ({_player2.Credentials.Username}) had {card2.Name} Damage: {card2.Damage} => calculated Damage: {damage1}\n\n";
            }  
            else if (damage1 > damage2)
            {
                _battleLock += $"Player1 ({_player1.Credentials.Username}) win with: {card1.Name} Damage: {card1.Damage} => calculated Damage: {damage1} against\nPlayer2 ({_player2.Credentials.Username}): {card2.Name} Damage: {card2.Damage} => calculated Damage: {damage2}\n\n";
                _player1.Deck.Add(card2);
                _player2.Deck.Remove(card2);
            }
            else
            {
                _battleLock += $"Player2 ({_player2.Credentials.Username}) win with: {card2.Name} Damage: {card2.Damage} => calculated Damage: {damage2} against\nPlayer1 ({_player1.Credentials.Username}):{card1.Name} Damage: {card1.Damage} => calculated Damage: {damage1}\n\n";
                _player2.Deck.Add(card1);
                _player1.Deck.Remove(card1);
            }
        }

        void UpdateWinner(User winner)
        {
            winner.ScoreboardData.Wins += 1;
            winner.ScoreboardData.Elo += 3;
        }
        void UpdateLooser(User looser)
        {
            looser.ScoreboardData.Losses += 1;
            looser.ScoreboardData.Elo -= 5;
        }
    }
}
