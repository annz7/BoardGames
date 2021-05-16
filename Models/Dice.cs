using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameBox.Models
{
    public class Dice : Item
    {
        public int Number { get; set; }
        private const int EdgeNumber = 6;

        public Dice() 
        {
            Number = 1;
        }

        public void ThrowDice()
        {
            var random = new Random();
            Number = random.Next(1, EdgeNumber + 1);
        }

        public override string ToString()
        {
            return $"Dice : number : {Number} , Index : ";
        }
    }
}

