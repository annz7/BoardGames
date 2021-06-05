using System;
using System.ComponentModel.DataAnnotations;

namespace BoardGames.Domain.Repositories
{
    public class BoardGameDbo
    {
        [Key]
        public Guid Id { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
