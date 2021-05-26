using Microsoft.EntityFrameworkCore;

namespace BoardGames.Domain.Repositories
{
    public class BoardGameRepositoryContext : DbContext
    {
        public BoardGameRepositoryContext(DbContextOptions<Models.BoardGameContext> options)
            : base(options)
        {
        }

        public DbSet<BoardGameDbo> BoardGames { get; set; }
        public DbSet<BoardGameItemDbo> BoardGameItems { get; set; }
    }
}
