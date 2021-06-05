using System;
using System.ComponentModel.DataAnnotations;
using BoardGames.Domain.Models;

namespace BoardGames.Domain.Repositories
{
    public class BoardGameItemDbo
    {
        [Key]
        public Guid Id { get; set; }
        public Guid BoardGameId { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
    }
}
