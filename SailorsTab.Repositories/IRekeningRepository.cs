using System;
using SailorsTab.Domain;

namespace SailorsTab.Repositories
{
	public interface IRekeningRepository : IRepository
	{
		void Create(Rekening rekening);
		void Update(Rekening rekening);
		Rekening[] GetAll();
		Rekening FindByNaam(string naam);
		void ConsumptieAfrekenen(Rekening rekening, Consumptie consumptie);
		void Delete(string naam);
	}
}

