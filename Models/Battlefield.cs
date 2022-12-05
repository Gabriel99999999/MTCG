using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Models
{
    internal class Battlefield
    {
        public User Player1;
        public User Player2;

        public Battlefield(User player1, User player2)
        {
            Player1 = player1;
            Player2 = player2;
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
                    //Monster vs Monster ?
                    if (CompareCurrentFighter(card1, card2))
                    {
                        //Monster vs Monster
                        if (!MvsM(player1, player2))
                            Console.WriteLine($"Draw because:\nPlayer1 had: {card1.Element} {card1.Name} Damage: {card1.Damage} and\nPlayer2 had {card2.Element} {card2.Name} Damage: {card2.Damage}");
                    }
                    else
                    {
                        //fight with minimum one spell
                    }
                }
                /*
                //Remove the first card of the decks
                player1.Deck.Remove(card1);
                player2.Deck.Remove(card2);
                */

                round++;                
            }
            if (player1.Deck.Count is 0)
                return player2;
            else if (player2.Deck.Count is 0)
                return player1;
            else
                return null;
        }
        public static bool CompareElements(ElementType element1, ElementType element2)
        {
            return false;
        }

        public bool CompareMonsterType()
        {
            return false;
        }

        public bool CompareCurrentFighter(Card Card1, Card Card2)
        {
            if (Card1 is MonsterCard && Card2 is MonsterCard)
                return true;

            return false;
        }

        //returnt true falls eine Special Rule den Fight entscheidet
        public bool SpecialRule(User player1, User player2, Card card1, Card card2)
        {
            //speichert winningCard
            Card? winCard = null;

            CardType[] spells = { CardType.WaterSpell, CardType.FireSpell, CardType.RegularSpell };

            //Goblin vs Dragon
            if (card1.Name is CardType.Goblin && card2.Name is CardType.Dragon)
                winCard = card2;
            else if(card1.Name is CardType.Wizzard && card2.Name is CardType.Ork)
                winCard = card1;

            //Wizzard vs Ork
            if (card1.Name is CardType.Ork && card2.Name is CardType.Wizzard)
                winCard = card2;
            else if (card1.Name is CardType.Dragon && card2.Name is CardType.Goblin)
                winCard = card1;

            //Knight vs WaterSpell
            if (card1.Name is CardType.Knight && card2.Name is CardType.WaterSpell)
                winCard = card2;
            else if (card1.Name is CardType.WaterSpell && card2.Name is CardType.Knight)
                winCard = card1;

            //Kraken vs a Spell
            if (card1.Name is CardType.Kraken && Array.Exists(spells, x => x == card2.Name) )
                winCard = card2;
            else if (Array.Exists(spells, spell => spell == card1.Name) && card2.Name is CardType.Kraken)
                winCard = card1;

            //FireElves vs Dragons
            if (card1.Name is CardType.Dragon && (card2.Element is ElementType.Fire && card2.Name is CardType.Elve) )
                winCard = card2;
            else if ( (card1.Element is ElementType.Fire && card1.Name is CardType.Elve) && card2.Name is CardType.Dragon)
                winCard = card1;

            if(winCard is not null)
            {
                if (winCard == card1)
                {
                    Console.WriteLine($"Player1 hat mit {card1.Element} {card1.Name} Damage: {card1.Damage} gegen {card2.Element} {card2.Name} Damage: {card2.Damage} gewonnen");
                    player1.Deck.Add(card2);
                }
                else
                {
                    Console.WriteLine($"Player2 hat mit {card2.Element} {card2.Name} Damage: {card2.Damage} gegen {card1.Element} {card1.Name} Damage: {card1.Damage} gewonnen");
                    player2.Deck.Add(card1);
                }
                return true;
            }

            return false;
        }

        public bool MvsM (User player1, User player2)
        {
            Card card1 = player1.Deck[1];
            Card card2 = player2.Deck[1];

            if (card1.Damage == card2.Damage)
                return false;
            if (card1.Damage > card2.Damage)
            {
                Console.WriteLine($"Player1 hat mit {card1.Element} {card1.Name} Damage: {card1.Damage} gegen {card2.Element} {card2.Name} Damage: {card2.Damage} gewonnen");
                player1.Deck.Add(card2);
            }
            else
            {
                Console.WriteLine($"Player2 hat mit {card2.Element} {card2.Name} Damage: {card2.Damage} gegen {card1.Element} {card1.Name} Damage: {card1.Damage} gewonnen");
                player2.Deck.Add(card1);
            }
            return true;
        }
    }
}
