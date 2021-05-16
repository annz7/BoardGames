using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameBox.Models
{
    public class Card : Item
    {
        public CardRank Rank { get; }
        public CardSuit Suit { get; set; }

        public Card(CardSuit suit, CardRank rank)
        {
            Suit = suit;
            Rank = rank;
        }
        public void MoveToIndex(Deck deck, int index)
        {
            deck.Cards.Remove(this);
            deck.Cards.Insert(index, this);
        }

        public override string ToString()
        {
            return $"Id : {Id}, Card, rank : {Rank}, suit : {Suit}, Index : ";
        }

        #region ForGameLogic
        //public CardColor Color
        //{
        //    get
        //    {
        //        if ((Suit == CardSuit.Spades) || (Suit == CardSuit.Clubs))
        //            return CardColor.Black;
        //        return CardColor.Red;
        //    }
        //}

        //public int Number => (int)Rank;

        //public string NumberString
        //{
        //    get
        //    {
        //        return Rank switch
        //        {
        //            CardRank.Ace => "A",
        //            CardRank.Jack => "J",
        //            CardRank.Queen => "Q",
        //            CardRank.King => "K",
        //            _ => Number.ToString()
        //        };
        //    }
        //}



        #endregion
    }
}
