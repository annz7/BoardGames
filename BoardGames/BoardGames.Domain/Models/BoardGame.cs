using System;
using System.Collections.Generic;
using System.Linq;

namespace BoardGames.Domain.Models
{
	public class BoardGame
	{
		public BoardGameId Id { get; }
		public int Width { get; set; }
        public int Height { get; set; }
        public List<List<BoardGameItem>> Items { get; }
        public BoardGame(int width = 8, int height = 8)
        {
            Id = new BoardGameId
            {
                Id = Guid.Empty
            };

            Width = width;
            Height = height;

            for (var i = 0; i < width; i++)
                Items.Add(new List<BoardGameItem>(height));
        }

		public void AddItem(BoardGameItemPosition position, BoardGameItem item)
        {
            if (IsPositionAvailable(position))
                throw new Exception("позиция недоступна");
            Items[position.X][position.Y] = item;
        }
		
		public BoardGameItem RemoveItem(BoardGameItemPosition position) // может стоит просто передовать BoardGameItem?
        {
            var item = Items[position.X][position.Y];
            Items[position.X][position.Y] = null;
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
                    && Items[position.X][position.Y] == null;
        }

    }
}