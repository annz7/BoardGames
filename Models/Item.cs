namespace GameBox.Models
{

    public abstract class Item
    {
        public int Id { get; set; }

        protected Item()
        {
            Id = Board.CountItems;
            Board.CountItems++;
        }
    }
}
