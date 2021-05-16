using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameBox.Models
{
    public class Board
    { 
        public List<Item> Items { get; set; }
        internal static Random random = new Random();
        internal static int CountItems = 0;
        internal Arm CurrentArm;


        public Board() => Items = new List<Item>();

        public void AddItem(Item item) => Items.Add(item);
        public Item FindItemById(long id)
        {
            foreach (var item in Items.Where(item => item.Id == id))
                return item;

            throw new Exception("There is not such item");
        }

        public bool IsEmptyCards()
        {
            return !Items.OfType<Card>().Any();
        }

        public List<T> GetItems<T>() where T : Item
        {
            return Items.OfType<T>().Select(item => item as T).ToList();
        }

        public void ChangeArm(Arm newArm)
        {
            CurrentArm = newArm;
        }
    }
}
