namespace MTCGServer.Models
{
    internal abstract class Card
    {
        public bool Fightable { get; set; }
        public  ElementType Element { get; set; }
        public CardType Name { get; set; }
        public float Damage { get; set; }
        public string Id { get; set; }

        protected Card(string id, string name, float damage)
        {
            Id = id;
            Name = Enum.Parse<CardType> (name);
            Damage = damage;
            Element = getElementOutOfName(Name);
            Fightable = true;

        }
        private ElementType getElementOutOfName(CardType name)
        {
            //für Element fire
            if (name is CardType.Dragon     ||
                name is CardType.Wizzard    ||
                name is CardType.FireElf    ||
                name is CardType.FireGoblin ||
                name is CardType.FireSpell  ||
                name is CardType.FireTroll)
            {
                return ElementType.Fire;
            }
            //für Element water
            if (name == CardType.Kraken ||
                name is CardType.WaterElf ||
                name is CardType.WaterGoblin ||
                name is CardType.WaterSpell ||
                name is CardType.WaterTroll)
            {
                return ElementType.Water;
            }
            else
            {
                return ElementType.Normal;
            }


        }
    }
}
