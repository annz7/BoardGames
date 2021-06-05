using System;

namespace BoardGames.Domain.Models
{
	public class BoardGameItemPosition
	{
        public int X { get; set; }
		public int Y { get; set; }
        public override bool Equals(object obj)
        {
            return Equals(obj as BoardGameItemPosition);
        }

        public bool Equals(BoardGameItemPosition other)
        {
            return other != null &&
                   X == other.X &&
                   Y == other.Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }
}