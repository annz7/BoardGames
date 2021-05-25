using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace BoardGames.Domain.Models
{
    public class BoardGameContext : DbContext
    {
        public BoardGameContext(DbContextOptions<BoardGameContext> options)
            : base(options)
        {
        }

        public DbSet<BoardGame> BoardGames { get; set; }
    }
}
