using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GameBox.Models
{
    public class Deck : Item
    {
        public List<Card> Cards { get; set; }

        public Deck()
        {
            Cards = new List<Card>();
        }

        public Card GetTopCard()
        {
            var first = Cards.First();
            Cards.RemoveAt(0);
            return first;
        }
        public void AddCard(Card card, int index)
        {
            Cards.Insert(index, card);
        }
        public bool IsEmpty()
        {
            return Cards.Count == 0;
        }
        public void Shuffle()
        {
            for(var i = 0; i < Cards.Count; i++)
            {
                MoveToIndex(Cards[i], new Random().Next(0, Cards.Count));
            }
        }
        public void MoveToIndex(Card card, int index)
        {
            Cards.Remove(card);
            Cards.Insert(index, card);
        }

        internal void CreateDeck()
        {
            for(var i = CardSuit.Spades; i <= CardSuit.Clubs; i++)
                for (var j = CardRank.Ace; j <= CardRank.King; j++)
                    Cards.Add(new Card(i, j));
        }

        public override string ToString()
        {
            return $"Id : {Id}, Deck, Index :";
        }
    }
}
