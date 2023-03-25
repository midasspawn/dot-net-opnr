using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AppOpener.Data.Interfaces;
using AppOpener.Data.Repositories;

namespace AppOpener.Data
{
	public static class Extentions
	{
		public static void AddUnitOfWork<Context>(this IServiceCollection services)
			where Context : DbContext
		{
			services.AddTransient(typeof(EntityRepository<,>), typeof(EntityRepository<,>));
			services.AddScoped<IUnitOfWork, UnitOfWork<Context>>();
		}
	}
}
