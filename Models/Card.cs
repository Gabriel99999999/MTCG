namespace MTCG.Models
{
    internal abstract class Card
    {
        public  ElementType Element { get; set; }
        public CardType Name { get; set; }
        public float Damage { get; set; }
        public string Id { get; set; }

        private ElementType getElementOutOfName(string name)
        {
            if (name == CardType.Dragon.ToString() || 
                name is )
                return ElementType.Fire;
            char[] delimiterChars = {  };
            if (name.Substring())
        }
    }
}
