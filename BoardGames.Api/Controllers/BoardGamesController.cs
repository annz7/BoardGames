using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BoardGames.Domain.Models;
using BoardGames.Domain.Repositories;

namespace BoardGames.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BoardGamesController : ControllerBase
    {
        private readonly IBoardGameRepository repository;

        public BoardGamesController(IBoardGameRepository repository)
        {
            this.repository = repository;
        }

        // GET: BoardGames
        [HttpGet]
        public IEnumerable<BoardGame> GetBoardGames()
        {
            return repository.GetAll();
        }

        // GET: BoardGames/5
        [HttpGet("{id}")]
        public BoardGame GetBoardGame(Guid id)
        {
            var boardGame = await _context.BoardGames.FindAsync(id);

            return boardGame;
        }

        [HttpGet("{id}/items")]
        public async Task<ActionResult<IEnumerable<BoardGameItem>>> GetItems(Guid id)
        {
            var boardGame = await _context.BoardGames.FindAsync(id);

            if (boardGame == null)
                return NotFound();
            
            return boardGame.Items;
        }

        [HttpPost]
        public async Task<ActionResult<BoardGame>> PostBoardGame(int width = 8, int height = 8)
        {
            var boardGame = new BoardGame(width, height);

            await _context.BoardGames.AddAsync(boardGame);
            await _context.SaveChangesAsync();

            return boardGame;
        }

        [HttpGet("{id}/items/{position}")]
        public async Task<ActionResult<BoardGameItem>> GetItems(Guid id, BoardGameItemPosition position)
        {
            var boardGame = await _context.BoardGames.FindAsync(id);

            if (boardGame == null)
                return NotFound();

            return boardGame.GetItem(position);
        }

        //[HttpPost("{id}/items/add")]
        //public async Task<ActionResult<BoardGame>> AddItem(Guid id, [FromBody]BoardGameItem item)
        //{
        //    var boardGame = await _context.BoardGames.FindAsync(id);

        //    if (boardGame == null)
        //        return NotFound();
        //    _context.BoardGames.Remove(boardGame);

        //    boardGame.AddItem(item.Position, item);

        //    await _context.BoardGames.AddAsync(boardGame);
        //    await _context.SaveChangesAsync();

        //    return boardGame;
        //}

        //[HttpPost("{id}/items/move")]
        //public async Task<ActionResult<BoardGame>> MoveItem(Guid id, 
        //    BoardGameItemPosition oldPosition, BoardGameItemPosition newPosition)
        //{
        //    var boardGame = await _context.BoardGames.FindAsync(id);

        //    if (boardGame == null)
        //        return NotFound();
        //    _context.BoardGames.Remove(boardGame);

        //    boardGame.InteractItem(oldPosition, newPosition);

        //    await _context.BoardGames.AddAsync(boardGame);
        //    await _context.SaveChangesAsync();

        //    return boardGame;
        //}

        // DELETE: BoardGames/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<BoardGame>> DeleteBoardGame(Guid id)
        {
            var boardGame = await _context.BoardGames.FindAsync(id);
            if (boardGame == null)
            {
                return NotFound();
            }

            _context.BoardGames.Remove(boardGame);
            await _context.SaveChangesAsync();

            return boardGame;
        }

        //[HttpDelete("{id}/items")]
        //public async Task<ActionResult<BoardGame>> DeleteItem(Guid id, BoardGameItemPosition position)
        //{
        //    var boardGame = await _context.BoardGames.FindAsync(id);
        //    if (boardGame == null)
        //        return NotFound();

        //    _context.BoardGames.Remove(boardGame);

        //    if (!boardGame.IsPositionInBoard(position))
        //        return BadRequest();

        //    boardGame.RemoveItem(position);

        //    await _context.BoardGames.AddAsync(boardGame);
        //    await _context.SaveChangesAsync();

        //    return boardGame;
        //}

        private bool BoardGameExists(Guid id)
        {
            return _context.BoardGames.Any(e => e.Id == id);
        }
    }
}

#region MaybeItsUseful
// PUT: BoardGames/5
//[HttpPut("{id}")]
//public async Task<IActionResult> PutBoardGame(Guid id, BoardGame boardGame)
//{
//    if (id != boardGame.Id)
//    {
//        return BadRequest();
//    }

//    _context.Entry(boardGame).State = EntityState.Modified;

//    try
//    {
//        await _context.SaveChangesAsync();
//    }
//    catch (DbUpdateConcurrencyException)
//    {
//        if (!BoardGameExists(id))
//        {
//            return NotFound();
//        }
//        else
//        {
//            throw;
//        }
//    }

//    return NoContent();
//}
#endregion