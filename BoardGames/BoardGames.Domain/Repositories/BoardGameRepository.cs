using BoardGames.Domain.Models;
using BoardGames.Infrastructure;

namespace BoardGames.Domain.Repositories
{
	public class BoardGameRepository: Repository<BoardGameId, BoardGame>
	{
	}
}