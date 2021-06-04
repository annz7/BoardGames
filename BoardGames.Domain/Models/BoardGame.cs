using System;
using System.Collections.Generic;

namespace BoardGames.Domain.Models
{
	public class BoardGame
	{
        public Guid Id { get; set; } 
        public int Width { get; set; }
        public int Height { get; set; }
        public IDictionary<BoardGameItemPosition, BoardGameItem> Items { get; set; }
        public BoardGame(int width = 8, int height = 8)
        {
            Id = Guid.NewGuid();

            Width = width;
            Height = height;

            Items = new Dictionary<BoardGameItemPosition, BoardGameItem>();
        }

        public BoardGameItem GetItem(BoardGameItemPosition position)
        {
            if (IsPositionInBoard(position))
                throw new Exception("позиция за пределами доски");
            return Items[position];
        }

        public void AddItem(BoardGameItem item)
        {
            if (IsPositionAvailable(item.Position))
                throw new Exception("позиция недоступна");
            Items[item.Position] = item;
        }
		
		public BoardGameItem RemoveItem(BoardGameItemPosition position) 
        {
            var item = Items[position];
            Items[position] = new BoardGameItem(position);
            return item;
        }

        public void InteractItem(BoardGameItemPosition oldPosition, BoardGameItemPosition newPosition)
        {
            if (IsPositionAvailable(newPosition))
                throw new Exception("позиция недоступна");

            var item = RemoveItem(oldPosition);
            item.Position = newPosition;
            AddItem(item);
        }

        public bool IsPositionAvailable(BoardGameItemPosition position)
        {
            return IsPositionInBoard(position)
                    && Items.ContainsKey(position);
        }

        public bool IsPositionInBoard(BoardGameItemPosition position)
        {
            return position.X >= 0 
                   && position.X < Width
                   && position.Y >= 0 
                   && position.Y < Height;
        }
    }
}