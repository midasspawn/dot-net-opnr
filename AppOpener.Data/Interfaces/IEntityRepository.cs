﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace AppOpener.Data.Interfaces
{
	public interface IEntityRepository<TEntity>
		where TEntity : class
	{
		void Add(TEntity entity);

		Task AddAsync(TEntity entity);

		Task AddRangeAsync(IEnumerable<TEntity> entity);

		List<TEntity> GetAll();

		Task<List<TEntity>> GetAllAsync();

		TEntity Get<TIdentity>(TIdentity id);

		Task<TEntity> GetAsync<TIdentity>(TIdentity id);

		void Attach(TEntity entity);
		void AddRange(IEnumerable<TEntity> entity);
		void AttachRange(IEnumerable<TEntity> entities);

		void Remove(TEntity entity);

		void RemoveRange(IEnumerable<TEntity> entities);

		void Update(TEntity entity);

		void UpdateRange(IEnumerable<TEntity> entities);

		IQueryable<TEntity> GetQuery();
	}
}
