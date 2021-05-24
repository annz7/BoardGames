using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BoardGames.Domain.Models
{
	public class BoardGame
	{
        [Key]
        public Guid Id { get; set; } //BoardGameId
        public int Width { get; set; }
        public int Height { get; set; }
        public List<BoardGameItem> Items { get; set; }
        public BoardGame(int width = 8, int height = 8)
        {
            Id = Guid.Empty;

            Width = width;
            Height = height;

            Items = new List<BoardGameItem>(width * height);
        }

		public void AddItem(BoardGameItemPosition position, BoardGameItem item)
        {
            if (IsPositionAvailable(position))
                throw new Exception("позиция недоступна");
            Items[position.X + Width * position.Y] = item;
        }
		
		public BoardGameItem RemoveItem(BoardGameItemPosition position) 
        {
            var item = Items[position.X + Width * position.Y];
            Items[position.X + Width * position.Y] = null;
            return item;
        }

        public void InteractItem(BoardGameItemPosition oldPosition, BoardGameItemPosition newPosition)
        {
            if (IsPositionAvailable(newPosition))
                throw new Exception("позиция недоступна");

            var item = RemoveItem(oldPosition);
            AddItem(newPosition, item);
        }

        public bool IsPositionAvailable(BoardGameItemPosition position)
        {
            return position.X >= 0 && position.X < Width
                    && position.Y >= 0 && position.Y < Height
                    && Items[position.X + Width * position.Y] == null;
        }

    }
}