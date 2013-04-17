using System;
namespace SailorsTab.Repositories
{
	public interface IRepositoryFactory
	{
		T CreateRepository<T>() where T : IRepository;
	}
}

