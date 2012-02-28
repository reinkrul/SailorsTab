using System;
using System.Data;

namespace SailorsTab.Repositories
{
	public class RepositoryFactory : IRepositoryFactory
	{
		private IDbConnection connection;
		
		public RepositoryFactory(IDbConnection connection)
		{
			this.connection = connection;
		}
		
		public T CreateRepository<T>() where T : IRepository
		{
			Type type = typeof(T);
			
			if (type == typeof(IRekeningRepository))
			{
				return (T)(IRepository)new RekeningRepository(connection);
			}
			else if (type == typeof(IConsumptieRepository))
			{
				return (T)(IRepository)new ConsumptieRepository(connection);
			}
			
			throw new Exception("Cannot create repository " + typeof(T));
		}
	}
}

