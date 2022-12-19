using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCGServer.Models
{
    internal class Battlefield
    {
        private User _player1;
        private User _player2;

        public Battlefield(User player1, User player2)
        {
            _player1 = player1;
            _player2 = player2;
        }

        public User? Fight(User player1, User player2)
        {
            int round = 0;
            Card card1, card2;
            Random random = new Random();
            int amount1 = 0;
            int amount2 = 0;

            while (player1.Deck.Count > 0 && player2.Deck.Count > 0 && round <= 100)
            {
                amount1 = player1.Deck.Count;
                amount2 = player2.Deck.Count;
                card1 = player1.Deck[random.Next(amount1)];
                card2 = player2.Deck[random.Next(amount2)];
                if(!SpecialRule(player1, player2, card1, card2))
                {
                    //is the fight influenced by the elements ?
                    if (CompareCurrentFighter(card1, card2))
                    {
                        notElementInfluencedF(player1, player2, card1, card2);
                    }
                    else
                    {
                        //fight which is influenced by the elements
                        elementInfluencedF(player1, player2, card1, card2);
                    }
                }

                round++;                
            }
            if (player1.Deck.Count is 0)
                return player2;
            else if (player2.Deck.Count is 0)
                return player1;
            else
                return null;
        }
        public bool CompareCurrentFighter(Card card1, Card card2)
        {
            if ( (card1 is MonsterCard && card2 is MonsterCard) || (card1.Element == card2.Element))
                return true;

            return false;
        }

        //returnt true falls eine Special Rule den Fight entscheidet
        public bool SpecialRule(User player1, User player2, Card card1, Card card2)
        {
            CardType[] goblins = { CardType.WaterGoblin, CardType.FireGoblin, CardType.RegularGoblin };
            //speichert winningCard
            Card? winCard = null;

            CardType[] spells = { CardType.WaterSpell, CardType.FireSpell, CardType.RegularSpell };

            //Bedigungen wo card1 gewinnt
            //Dragon vs Goblin || Wizzard vs Ork || WaterSpell vs Knight || Kraken vs spells || FireElve vs Dragon
            if ( (card1.Name is CardType.Dragon && Array.Exists(goblins, goblin => goblin == card2.Name) )  ||
                 (card1.Name is CardType.Wizzard && card2.Name is CardType.Ork)                             ||
                 (card1.Name is CardType.WaterSpell && card2.Name is CardType.Knight)                       ||
                 (card1.Name is CardType.Kraken && Array.Exists(spells, x => x == card2.Name)               ||
                 (card1.Name is CardType.FireElf) && card2.Name is CardType.Dragon) )
            {
                winCard = card1;
            }
            //Bedigungen wo card2 gewinnt
            //Goblin vs Dragon || Ork vs Wizzard || Knight vs WaterSpell || spells vs Kraken ||Dragon vs FireElve
            else if ( (Array.Exists(goblins, goblin => goblin == card1.Name) && card2.Name is CardType.Dragon)  ||
                      (card1.Name is CardType.Ork && card2.Name is CardType.Wizzard)                            ||
                      (card1.Name is CardType.Knight && card2.Name is CardType.WaterSpell)                      ||
                      (Array.Exists(spells, spell => spell == card1.Name) && card2.Name is CardType.Kraken)     ||
                      (card1.Name is CardType.Dragon && card2.Name is CardType.FireElf))
            {
                winCard = card2;
            }

            if(winCard is not null)
            {
                if (winCard == card1)
                {
                    Console.WriteLine($"Player1 win with: {card1.Element} {card1.Name} Damage: {card1.Damage} against\nPlayer2 with: {card2.Element} {card2.Name} Damage: {card2.Damage}");
                    player1.Deck.Add(card2);
                }
                else
                {
                    Console.WriteLine($"Player2 win with: {card2.Element} {card2.Name} Damage: {card2.Damage} against\nPlayer1 with: {card1.Element} {card1.Name} Damage: {card1.Damage}");
                    player2.Deck.Add(card1);
                }
                return true;
            }

            return false;
        }

        public void notElementInfluencedF(User player1, User player2, Card card1, Card card2)
        {
            if (card1.Damage == card2.Damage)
                Console.WriteLine($"Draw because:\nPlayer1 had: {card1.Element} {card1.Name} Damage: {card1.Damage} and\nPlayer2 had {card2.Element} {card2.Name} Damage: {card2.Damage}");
            if (card1.Damage > card2.Damage)
            {
                Console.WriteLine($"Player1 win with: {card1.Element} {card1.Name} Damage: {card1.Damage} against\nPlayer2 with: {card2.Element} {card2.Name} Damage: {card2.Damage}");
                player1.Deck.Add(card2);
            }
            else
            {
                Console.WriteLine($"Player2 win with: {card2.Element} {card2.Name} Damage: {card2.Damage} against\nPlayer1 with: {card1.Element} {card1.Name} Damage: {card1.Damage}");
                player2.Deck.Add(card1);
            }
        }

        public void elementInfluencedF(User player1, User player2, Card card1, Card card2)
        {
            float damage1 = card1.Damage;
            float damage2 = card2.Damage;
            //Bedingungen die card1 Damage verdoppeln und card2 Damage halbiert
            //Water vs Fire || Fire vs Normal || Normal vs Water
            if( (card1.Element is ElementType.Water) && (card2.Element is ElementType.Fire) ||
                (card1.Element is ElementType.Fire) && (card2.Element is ElementType.Normal) ||
                (card1.Element is ElementType.Normal) && (card2.Element is ElementType.Water) )
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
                Console.WriteLine($"Draw because:\nPlayer1 had: {card1.Element} {card1.Name} Damage: {card1.Damage} => calculated Damage: {damage1} and\nPlayer2 had {card2.Element} {card2.Name} Damage: {card2.Damage} => calculated Damage: {damage1}");
            if (damage1 > damage2)
            {
                Console.WriteLine($"Player1 win with: {card1.Element} {card1.Name} Damage: {card1.Damage} => calculated Damage: {damage1} against\nPlayer2: {card2.Element} {card2.Name} Damage: {card2.Damage} => calculated Damage: {damage2}");
                player1.Deck.Add(card2);
            }
            else
            {
                Console.WriteLine($"Player2 win with: {card2.Element} {card2.Name} Damage: {card2.Damage} => calculated Damage: {damage2} against\nPlayer2: {card1.Element} {card1.Name} Damage: {card1.Damage} => calculated Damage: {damage1}");
                player2.Deck.Add(card1);
            }
        }
    }
}
