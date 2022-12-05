using MTCG.Models;
using MTCG.Models.Monster;

class Program
{
    static void Main(string[] args)
    {
        List<Card> package1 = new List<Card>();

        Card Card1 = new MonsterCard(ElementType.Fire, CardType.Goblin, 10);
        Card Card2 = new MonsterCard(ElementType.Fire, CardType.Dragon, 10);
        Card Card3 = new MonsterCard(ElementType.Water, CardType.Kraken, 10);
        Card Card4 = new MonsterCard(ElementType.Normal, CardType.Knight, 10);
        Card Card5 = new MonsterCard(ElementType.Normal, CardType.Goblin, 10);
        Card Card6 = new MonsterCard(ElementType.Water, CardType.Goblin, 10);
        Card Card7 = new MonsterCard(ElementType.Fire, CardType.Dragon, 15);
        Card Card8 = new MonsterCard(ElementType.Water, CardType.Kraken, 10);
        Card Card9 = new MonsterCard(ElementType.Normal, CardType.Wizzard, 10);
        Card Card10 = new MonsterCard(ElementType.Fire, CardType.Elve, 10);

        package1.Add(Card10);
        package1.Add(Card9);
    }

    
}