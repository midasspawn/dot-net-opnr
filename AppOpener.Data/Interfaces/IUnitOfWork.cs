using System.Threading.Tasks;

namespace AppOpener.Data.Interfaces
{
	public interface IUnitOfWork
	{
		IEntityRepository<TEntity> Repository<TEntity>() where TEntity : class;
		Task<int> SaveChangesAsync();
		int SaveChanges();
	}
}
