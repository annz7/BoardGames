namespace BoardGames.Domain.Models
{
	public class BoardGameItem
	{
        public BoardGameItemPosition Position { get; set; }

        public BoardGameItem(BoardGameItemPosition position)
        {
            Position.X = position.X;
            Position.Y = position.Y;
        }
    }
}