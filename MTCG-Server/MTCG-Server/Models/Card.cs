

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace MTCGServer.Models
{
    public class Card
    {
        [IgnoreDataMember]
        public bool Fightable { get; set; }
        [IgnoreDataMember]
        public  ElementType Element { get; set; }

        [IgnoreDataMember]
        public CardType NameEnum  { get; set; }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Damage { get; set; }
        

        public Card(Guid id, string name, decimal damage)
        {
            Id = id;
            Name = name;
            NameEnum = Enum.Parse<CardType> (name);
            Damage = damage;
            Element = getElementOutOfName(NameEnum);
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
