using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public IEnumerable<BoardGame> GetBoardGames()
        {
            return repository.GetAll();
        }

        [HttpGet("{id}")]
        public BoardGame GetBoardGame(Guid id)
        {
            return repository.Get(id);
        }

        [HttpGet("{id}/items")]
        public IEnumerable<BoardGameItem> GetItems(Guid id)
        {
            var boardGame = repository.Get(id);

            foreach (var item in boardGame.Items)
                yield return item;
        }

        [HttpGet("{id}/items/{positionX}_{positionY}")]
        public BoardGameItem GetItem(
            [FromRoute] Guid id,
            [FromRoute] int positionX,
            [FromRoute] int positionY)
        {
            return repository.Get(id).GetItem(new BoardGameItemPosition() { X = positionX, Y = positionY });
        }

        [HttpPost]
        public BoardGame CreateBoardGame(int width = 8, int height = 8)
        {
            var boardGame = new BoardGame(width, height);

            repository.Add(boardGame);

            return boardGame;
        }

        [HttpPost("{id}/items/{positionX}_{positionY}/add")]
        public BoardGame AddItem(
            [FromRoute] Guid id, 
            [FromRoute] int positionX,
            [FromRoute] int positionY)
        {
            var boardGame = repository.Get(id);

            boardGame.AddItem(new BoardGameItem(new BoardGameItemPosition() { X = positionX, Y = positionY }));

            repository.Update(boardGame);

            return boardGame;
        }

        [HttpPost("{id}/items/{oldPositionX}_{oldPositionY}/move")]
        public BoardGame MoveItem([FromRoute] Guid id,
            [FromRoute] int oldPositionX, 
            [FromRoute] int oldPositionY,
            [FromBody] BoardGameItemPosition newPosition)
        {
            var boardGame = repository.Get(id);

            boardGame.InteractItem(new BoardGameItemPosition() { X = oldPositionX, Y = oldPositionY }, newPosition);

            repository.Update(boardGame);

            return boardGame;
        }

        [HttpDelete("{id}")]
        public  BoardGame DeleteBoardGame(Guid id)
        {
            var boardGame = repository.Get(id);

            repository.Remove(boardGame);

            return boardGame;
        }

        [HttpDelete("{id}/items/{positionX}_{positionY}")]
        public BoardGame DeleteItem(
            [FromRoute] Guid id, 
            [FromRoute] int positionX,
            [FromRoute] int positionY)
        {
            var position = new BoardGameItemPosition() { X = positionX, Y = positionY };
            var boardGame = repository.Get(id);

            boardGame.RemoveItem(position);

            repository.Update(boardGame);

            return boardGame;
        }

        private bool BoardGameExists(Guid id)
        {
            return repository.GetAll().Any(e => e.Id == id);
        }
    }
}