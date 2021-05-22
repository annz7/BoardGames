using System;
using System.Collections.Generic;

namespace BoardGames.Infrastructure
{
	public class Repository<TId, TEntity>
	{
		public TEntity Find(TId id)
		{
			throw new NotImplementedException();
        }
		
		public IEnumerable<TEntity> FindAll()
		{
			throw new NotImplementedException();
		}
		
		public void Save(TId id, TEntity entity)
		{
			throw new NotImplementedException();
		}
	}
}