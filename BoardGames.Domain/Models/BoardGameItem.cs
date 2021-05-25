using System;
using System.ComponentModel.DataAnnotations;

namespace BoardGames.Domain.Models
{
	public class BoardGameItem
	{
        [Key]
        public Guid Id { get; set; }
        public BoardGameItemPosition Position { get; set; }

        public BoardGameItem()
        {
            Id = Guid.Empty;
        }
        public BoardGameItem(BoardGameItemPosition position)
        {
            Id = Guid.Empty;
            Position.X = position.X;
            Position.Y = position.Y;
        }
	}
}