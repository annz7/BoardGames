using System;

namespace BoardGames.Domain.Models
{
	public class BoardGameItem
	{
        public Guid Id { get; set; }
        public BoardGameItemPosition Position { get; set; }

		public BoardGameItem(BoardGameItemPosition position)
        {
            Id = Guid.Empty;
            Position.X = position.X;
            Position.Y = position.Y;
        }
	}
}