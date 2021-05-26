using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoardGames.Domain.Models;

namespace BoardGames.Domain.Repositories
{
    public interface IBoardGameRepository
    {
        void Save(BoardGame boardGame);
        BoardGame Find(Guid boardGameId);
        IEnumerable<BoardGame> GetAll();
    }

    public class BoardGameRepository : IBoardGameRepository
    {
        private readonly BoardGameRepositoryContext _context;

        public BoardGameRepository(BoardGameRepositoryContext context)
        {
            _context = context;
        }

        public void Save(BoardGame boardGame)
        {
            _context.BoardGames.Add(Build(boardGame));
            _context.BoardGameItems.AddRange(BuildItems(boardGame));
            _context.SaveChangesAsync();
        }
        public BoardGame Find(Guid boardGameId)
        {
            var boardGameDbo = _context.BoardGames.Find(boardGameId);
            var boardGameItemDbos = _context.BoardGameItems
                .Where(y => y.BoardGameId == boardGameId)
                .ToList();
            return Build(boardGameDbo, boardGameItemDbos);
        }
        public IEnumerable<BoardGame> GetAll()
        {
            throw new NotImplementedException();
        }

        private static BoardGameDbo Build(BoardGame boardGame)
        {
            throw new NotImplementedException();
        }

        private static IEnumerable<BoardGameItemDbo> BuildItems(BoardGame boardGame)
        {
            throw new NotImplementedException();
        }

        private BoardGame Build(BoardGameDbo boardGameDbo, IEnumerable<BoardGameItemDbo> itemsDbos)
        {
            throw new NotImplementedException();
        }
    }
}
