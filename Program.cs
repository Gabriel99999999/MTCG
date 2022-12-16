using MTCG.Models;
using MTCG.Models.Monster;

class Program
{
    static void Main(string[] args)
    {
        Card card1 = new MonsterCard(ElementType.Fire, CardType.Goblin, 10);
        Card card2 = new MonsterCard(ElementType.Fire, CardType.Dragon, 10);
        Card card3 = new MonsterCard(ElementType.Water, CardType.Kraken, 10);
        Card card4 = new MonsterCard(ElementType.Normal, CardType.Knight, 10);
        Card card5 = new MonsterCard(ElementType.Normal, CardType.Goblin, 10);
        Card card6 = new MonsterCard(ElementType.Water, CardType.Goblin, 10);
        Card card7 = new MonsterCard(ElementType.Fire, CardType.Dragon, 15);
        Card card8 = new MonsterCard(ElementType.Water, CardType.Kraken, 10);
        Card card9 = new MonsterCard(ElementType.Normal, CardType.Wizzard, 10);
        Card card10 = new MonsterCard(ElementType.Fire, CardType.Elve, 10);

        Package package1 = new Package(card1, card2, card3, card4);
        Package package2 = new Package(null, null, null, null);
    }

    
}