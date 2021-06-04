using System;
using System.ComponentModel.DataAnnotations;
using BoardGames.Domain.Models;

namespace BoardGames.Domain.Repositories
{
    public class BoardGameItemDbo
    {
        [Key]
        public Guid BoardGameId { get; set; }
        public BoardGameItemPosition Position { get; set; }
    }
}
