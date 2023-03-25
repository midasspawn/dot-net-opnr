using Microsoft.EntityFrameworkCore;
using AppOpener.Data.Interfaces;
using AppOpener.Data.Repositories;
using System;
using System.Threading.Tasks;

namespace AppOpener.Data
{
	public class UnitOfWork<TDbContext> : IUnitOfWork where TDbContext : DbContext
	{
		protected readonly TDbContext _dbContext;
		protected readonly IServiceProvider _serviceProvider;

		public UnitOfWork(TDbContext context, IServiceProvider serviceProvider)
		{
			_dbContext = context;
			_serviceProvider = serviceProvider;
		}

		public IServiceProvider ServiceProvider { get; }

		public IEntityRepository<TEntity> Repository<TEntity>() where TEntity : class
		{
			var repositoryType = typeof(EntityRepository<TDbContext, TEntity>);
			return (IEntityRepository<TEntity>)_serviceProvider.GetService(repositoryType);
		}

		public Task<int> SaveChangesAsync()
		{
			
			return _dbContext.SaveChangesAsync();
		}
		public int SaveChanges()
		{

			return _dbContext.SaveChanges();
		}
	}
}
