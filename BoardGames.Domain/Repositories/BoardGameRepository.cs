using System;
using System.Collections.Generic;
using System.Linq;
using BoardGames.Domain.Models;

namespace BoardGames.Domain.Repositories
{
    public interface IBoardGameRepository
    {
        IEnumerable<BoardGame> GetAll();
        BoardGame Get(Guid boardGameId);
        void Remove(BoardGame boardGame);
        void Add(BoardGame boardGame);
        void Update(BoardGame boardGame);
    }

    public class BoardGameRepository : IBoardGameRepository
    {
        private readonly BoardGameRepositoryContext _context;

        public BoardGameRepository(BoardGameRepositoryContext context)
        {
            _context = context;
        }

        public IEnumerable<BoardGame> GetAll()
        {
            var boardGameDbos = _context.BoardGames.ToList();

            foreach (var boardGameDbo in boardGameDbos)
            {
                var boardGameItemDbos = _context.BoardGameItems
                    .Where(y => y.BoardGameId == boardGameDbo.Id)
                    .ToList();
                yield return Build(boardGameDbo, boardGameItemDbos);
            }
        }

        public BoardGame Get(Guid boardGameId)
        {
            var boardGameDbo = _context.BoardGames.Find(boardGameId);
            var boardGameItemDbos = _context.BoardGameItems
                .Where(y => y.BoardGameId == boardGameId)
                .ToList();
            return Build(boardGameDbo, boardGameItemDbos);
        }

        public void Add(BoardGame boardGame)
        {
            _context.BoardGames.Add(Build(boardGame));
            _context.BoardGameItems.AddRange(BuildItems(boardGame));
            _context.SaveChanges();
        }

        //public void Add(BoardGame boardGame)
        //{
        //    //_context.BoardGames.Add(Build(boardGame));
        //    var boardGameDbo = _context.BoardGames.Find(boardGame.Id);
        //    _context.BoardGames.Add(boardGameDbo);
        //    var items = _context.BoardGameItems
        //        .Where(y => y.BoardGameId == boardGameDbo.Id)
        //        .ToList();
        //    _context.BoardGameItems.AddRange(items);//BuildItems(boardGame));
        //    _context.SaveChanges();
        //}

        public void Remove(BoardGame boardGame)
        {
            var boardGameDbo = _context.BoardGames.Find(boardGame.Id);
            _context.BoardGames.Remove(boardGameDbo);

            var items = _context.BoardGameItems
                .Where(y => y.BoardGameId == boardGameDbo.Id)
                .ToList();
            _context.BoardGameItems.RemoveRange(items);

            _context.SaveChanges();
        }

        public void Update(BoardGame newBoardGame)
        {
            var oldBoardGameDbo = _context.BoardGames.Find(newBoardGame.Id);
            _context.BoardGames.Remove(oldBoardGameDbo);

            var items = _context.BoardGameItems
                .Where(y => y.BoardGameId == oldBoardGameDbo.Id)
                .ToList();
            _context.BoardGameItems.RemoveRange(items);

            _context.SaveChanges();

            Add(newBoardGame);
        }
        
        private static BoardGameDbo Build(BoardGame boardGame)
        {
            return new BoardGameDbo()
            {
                Id = boardGame.Id,
                Height = boardGame.Height,
                Width = boardGame.Width
            };
        }

        private static IEnumerable<BoardGameItemDbo> BuildItems(BoardGame boardGame)
        {
            return boardGame.Items.Select(item => BuildItem(item, boardGame.Id));
        }

        private static BoardGameItemDbo BuildItem(BoardGameItem item, Guid boardGameId)
        {
            return new BoardGameItemDbo()
            {
                BoardGameId = boardGameId,
                //Position = item.Position
                PositionX = item.Position.X,
                PositionY = item.Position.Y
            };
        }

        private static BoardGame Build(BoardGameDbo boardGameDbo, IEnumerable<BoardGameItemDbo> itemsDbos)
        {
            var boardGame = new BoardGame()
            {
                Id = boardGameDbo.Id,
                Height = boardGameDbo.Height,
                Width = boardGameDbo.Width
            };

            foreach (var itemDbo in itemsDbos)
                boardGame.AddItem(BuildItem(itemDbo));

            return boardGame;
        }

        private static BoardGameItem BuildItem(BoardGameItemDbo itemDbo)
        {
            return new BoardGameItem(
                new BoardGameItemPosition()
                {
                    X = itemDbo.PositionX,
                    Y = itemDbo.PositionY
                });
        }
    }
}
