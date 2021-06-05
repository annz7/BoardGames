﻿using System;
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

        [HttpGet("{id}/items/{position}")]
        public BoardGameItem GetItem(Guid id, BoardGameItemPosition position)
        {
            return repository.Get(id).GetItem(position);
        }

        [HttpPost]
        public BoardGame PostBoardGame(int width = 8, int height = 8)
        {
            var boardGame = new BoardGame(width, height);

            repository.Add(boardGame);

            return boardGame;
        }

        [HttpPost("{id}/items/{x}_{y}/add")]
        public BoardGame AddItem(Guid id, [FromRoute] int x, [FromRoute] int y)
        {
            var boardGame = repository.Get(id);

            boardGame.AddItem(new BoardGameItem(new BoardGameItemPosition{X = x, Y = y}));

            repository.Update(boardGame);

            return boardGame;
        }

        [HttpPost("{id}/items/{oldPositionX}_{oldPositionY}/{newPositionX}_{newPositionY}/move")]
        public BoardGame MoveItem([FromRoute] Guid id,
            [FromRoute] int oldPositionX,
            [FromRoute] int oldPositionY,
            [FromRoute] int newPositionX,
            [FromRoute] int newPositionY)
        {
            var boardGame = repository.Get(id);

            boardGame.InteractItem(new BoardGameItemPosition{
                                                    X = oldPositionX, 
                                                    Y = oldPositionY},
                                    new BoardGameItemPosition {
                                                    X = newPositionX,
                                                    Y = newPositionY});

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

        [HttpDelete("{id}/items/{x}_{y}")]
        public async Task<ActionResult<BoardGame>> DeleteItem(Guid id, [FromRoute] int x, [FromRoute] int y)
        {
            var boardGame = repository.Get(id);

            var position = new BoardGameItemPosition{X = x, Y = y};

            if (!boardGame.IsPositionInBoard(position))
                return BadRequest("Позиция за пределами доски");

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