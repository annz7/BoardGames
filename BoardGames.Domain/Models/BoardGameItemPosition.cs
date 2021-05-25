using System;
using System.ComponentModel.DataAnnotations;

namespace BoardGames.Domain.Models
{
	public class BoardGameItemPosition
	{
		[Key]
		public long Id { get; set; }
		public int X { get; set; }
		public int Y { get; set; }
	}
}