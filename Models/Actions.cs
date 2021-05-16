using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace GameBox.Models
{
    public class Actions
    {
        public static void ShuffleDeck(Deck deck) => deck.Shuffle();
        public static void TakeCardAtArmFromDeck(Deck deck, Arm arm)
        {
            var card = deck.GetTopCard();
            arm.AddCard(card);
        }
        public static void TakeCardAtArmFromBoard(Board board, Card card)
        { 
            board.CurrentArm.AddCard(card);
            board.Items.Remove(card);
        }
        public static void PutCardOnBoardFromArm(Board board, Card card)
        {
            board.CurrentArm.Cards.Remove(card);
            board.Items.Add(card);
        }

        public static void PutCardAtDeckFromArm(Arm arm, Deck deck, Card card, int index)
        {
            arm.Cards.Remove(card);
            deck.AddCard(card, index);
        }
    }
}
