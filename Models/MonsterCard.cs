using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Models
{
    internal class MonsterCard : Card
    {
        public MonsterCard(ElementType type, CardType name, int damage, string id)
        {   
            Element = getElement(name);
            Name = name;    
            Damage = damage;
            checkCard(type, name, damage);
            Id = id;
            
        }
        //Methode welche die Parameter beim MonsterCard Konstruktor überprüft und falsche Element typen richtig stellt
        private void checkCard(ElementType type, CardType name, int damage)
        {
            if(name is CardType.Dragon)
            {
                if(!(type is ElementType.Fire))
                {
                    //Evtl auch mit Exceptions lösen
                    Console.WriteLine("WARNING A Monster from Type Dragon can only be from the Element Type Fire");
                    type = ElementType.Fire;
                }
            }
            //es fehlt noch für die anderen special Monster und damage <=100 evtl
        }
    }
}
