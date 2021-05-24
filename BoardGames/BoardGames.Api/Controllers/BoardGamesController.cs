using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BoardGames.Domain.Models;

namespace BoardGames.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BoardGamesController : ControllerBase
    {
        private readonly BoardGameContext _context;

        public BoardGamesController(BoardGameContext context)
        {
            _context = context;
        }

        // GET: BoardGames
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BoardGame>>> GetBoardGames()
        {
            return await _context.BoardGames.ToListAsync();
        }

        // GET: BoardGames/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BoardGame>> GetBoardGame(Guid id)
        {
            var boardGame = await _context.BoardGames.FindAsync(id);

            if (boardGame == null)
            {
                return NotFound();
            }

            return boardGame;
        }

        // POST: BoardGames
        //[HttpPost]
        //public async Task<ActionResult<BoardGame>> PostBoardGame()
        //{
        //    var boardGame = new BoardGame()
        //    {
        //        Id = Guid.NewGuid()
        //    };

        //    _context.BoardGames.Add(boardGame);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetBoardGame", new { id = boardGame.Id }, boardGame);
        //}

        [HttpPost] // так работает
        public async Task<BoardGame> PostBoardGame(int width = 8, int height = 8)
        {
            var boardGame = new BoardGame(width, height)
            {
                Id = Guid.NewGuid()
            };

            _context.BoardGames.Add(boardGame);
            await _context.SaveChangesAsync();

            return boardGame;
        }

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

        private bool BoardGameExists(Guid id)
        {
            return _context.BoardGames.Any(e => e.Id == id);
        }
    }
}
