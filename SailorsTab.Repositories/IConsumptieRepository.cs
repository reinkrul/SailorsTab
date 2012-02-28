using System;
using SailorsTab.Domain;

namespace SailorsTab.Repositories
{
	public interface IConsumptieRepository : IRepository
	{
		void Create(Consumptie consumptie);
		void Delete(Consumptie consumptie);
		void Update(Consumptie consumptie);
		ConsumptieCategorie[] GetAllByCategorie();
		Consumptie[] GetAll();
	}
}

