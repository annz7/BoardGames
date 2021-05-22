using System;
using System.Collections.Generic;
using BoardGames.Domain.Models;
using BoardGames.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BoardGames.Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class BoardGamesController : ControllerBase
	{
		private readonly ILogger<BoardGamesController> _logger;
		private readonly BoardGameRepository _boardGameRepository;

		public BoardGamesController(ILogger<BoardGamesController> logger, BoardGameRepository boardGameRepository)
		{
			_logger = logger;
			_boardGameRepository = boardGameRepository;
		}

		[HttpGet]
		public IEnumerable<BoardGame> Get()
        {
            return _boardGameRepository.FindAll();
        }

        [HttpGet]
        [Route("{id}")]
        public BoardGame Get(BoardGameId id)
        {
            return _boardGameRepository.Find(id);
        }

        [HttpPost]
        public BoardGame Create()
        {
            var boardGame = new BoardGame();

            _boardGameRepository.Save(boardGame.Id, boardGame);

            return boardGame;
        }

        [HttpPost]
        public BoardGame Create(int weight, int height)
        {
            var boardGame = new BoardGame(weight, height);

            _boardGameRepository.Save(boardGame.Id, boardGame);

            return boardGame;
        }

        [HttpPost]
        [Route("{id}/items")]
        public BoardGame CreateItem(BoardGameId id, BoardGameItem item)
        {
            var boardGame = _boardGameRepository.Find(id);

            boardGame.AddItem(item.Position, item);

            _boardGameRepository.Save(boardGame.Id, boardGame);

            return boardGame;
        }

        [HttpDelete]
        [Route("{id}/items/{position}")]
        public BoardGame RemoveItem(BoardGameId id, BoardGameItemPosition position)
        {
            var boardGame = _boardGameRepository.Find(id);

            boardGame.RemoveItem(position);

            _boardGameRepository.Save(boardGame.Id, boardGame);

            return boardGame;
        }


        [HttpDelete]
        [Route("{id}")]
        public void Remove(BoardGameId id)
        {
            var boardGame = _boardGameRepository.Find(id);

            boardGame = null;

            _boardGameRepository.Save(boardGame.Id, boardGame);
        }
    }
}