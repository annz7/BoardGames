using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameBox.Models
{
    public class Arm : Item
    {
        public List<Card> Cards = new List<Card>();

        public void AddCard(Card card)
        {
            Cards.Add(card);
        }

        public List<Card> GetCards()
        {
            return Cards;
        }

        public bool IsEmpty()
        {
            return Cards.Count == 0;
        }

        public override string ToString()
        {
            return $"Id : {Id}, Player";
        }
    }
}
