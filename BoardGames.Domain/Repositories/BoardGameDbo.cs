using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

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
