namespace BoardGames.Domain.Models
{
    public class BoardGameItem
    {
        public BoardGameItemPosition Position { get; set; }

        public BoardGameItem() {}

        public BoardGameItem(BoardGameItemPosition position)
        {
            Position = position;
        }
    }
}
