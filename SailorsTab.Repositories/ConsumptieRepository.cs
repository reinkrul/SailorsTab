using System;
using SailorsTab.Domain;
using System.Data;
using System.Collections.Generic;

namespace SailorsTab.Repositories
{
	public class ConsumptieRepository : RepositoryBase, IConsumptieRepository
	{
        public ConsumptieRepository(IDbConnection connection)
            : base(connection)
        {
        }
		
		public void Create(Consumptie consumptie)
		{
			if (!consumptie.Validate())
			{
				throw new RepositoryException("Consumptie is ongeldig: " + consumptie);
			}
			
			IDbCommand command = connection.CreateCommand();
			command.CommandText = "INSERT INTO consumptie (Naam, Prijs) VALUES (@naam, @prijs)";
			command.AddParameter("@naam", consumptie.Naam);
			command.AddParameter("@prijs", consumptie.Prijs);
			command.Prepare();
			
			if (command.ExecuteNonQuery() != 1)
			{
				throw new RepositoryException("Kan consumptie '" + consumptie.Naam + "' niet aanmaken.");
			}

            Commit();
		}
		
		public void Delete(Consumptie consumptie)
		{
			if (consumptie.Id == 0)
			{
				throw new RepositoryException("Consumptie is ongeldig: " + consumptie);
			}
			
			IDbCommand command = connection.CreateCommand();
			command.CommandText = "DELETE FROM consumptie WHERE rowid = @id";
			command.AddParameter("@id", consumptie.Id);
			command.Prepare();
			
			if (command.ExecuteNonQuery() != 1)
			{
				throw new RepositoryException("Kan consumptie '" + consumptie.Naam + "' niet verwijderen.");
			}

            Commit();
		}
		
		public void Update(Consumptie consumptie)
		{
			if (consumptie.Id == 0)
			{
				throw new RepositoryException("Ongeldige consumptie.");
			}
			
			IDbCommand command = connection.CreateCommand();
			command.CommandText = "UPDATE consumptie SET naam = @naam, categorie = @categorie, prijs = @prijs WHERE rowid = @id";
			command.AddParameter("@naam", consumptie.Naam);
			command.AddParameter("@categorie", consumptie.Categorie);
			command.AddParameter("@prijs", consumptie.Prijs);
			command.AddParameter("@id", consumptie.Id);
			command.Prepare();
			
			command.ExecuteNonQuery();

            Commit();
		}
		
		public Consumptie[] GetAll()
		{
			IDbCommand command = connection.CreateCommand();
			command.CommandText = "SELECT rowid, * FROM consumptie ORDER BY categorie, naam";
			using (IDataReader reader = command.ExecuteReader())
			{
				List<Consumptie> consumpties = new List<Consumptie>();
				
				while(reader.Read())
				{
					Consumptie consumptie = readConsumptie(reader);
					consumpties.Add(consumptie);
				}
				
				return consumpties.ToArray();
			}
		}
		
		public ConsumptieCategorie[] GetAllByCategorie()
		{
			IDbCommand command = connection.CreateCommand();
			command.CommandText = "SELECT rowid, * FROM consumptie ORDER BY categorie, naam";
			using (IDataReader reader = command.ExecuteReader())
			{
				List<ConsumptieCategorie> categories = new List<ConsumptieCategorie>();
				
				if (reader.Read())
				{
					bool hasMoreRecords;
					do
					{
						string categorieNaam = reader.GetString("Categorie");
						ConsumptieCategorie categorie;
						hasMoreRecords = readCategorie(reader, categorieNaam, out categorie); 
						if (categorie != null)
						{
							categories.Add(categorie);
						}
					} while(hasMoreRecords);
				}
				
				return categories.ToArray();
			}
		}
		
		private bool readCategorie(IDataReader dataReader, string categorieNaam, out ConsumptieCategorie categorie)
		{
			List<Consumptie> consumpties = new List<Consumptie>();
			bool hasMoreRecords = false;
			do
			{	
				Consumptie consumptie = readConsumptie(dataReader);
				consumpties.Add(consumptie);
				
				hasMoreRecords = dataReader.Read();
			} while(hasMoreRecords && dataReader.GetString("Categorie") == categorieNaam);
			
			if (consumpties.Count > 0)
			{
				categorie = new ConsumptieCategorie(categorieNaam, consumpties.ToArray());
			}
			else
			{
				categorie = null;
			}
			

			return hasMoreRecords;
		}
		
		private Consumptie readConsumptie(IDataReader dataReader)
		{
			Consumptie consumptie = new Consumptie();
			consumptie.Id = dataReader.GetInt32("rowid");
			consumptie.Naam = dataReader.GetString("Naam");
			consumptie.Prijs = dataReader.GetDecimal("Prijs");
			consumptie.Categorie = dataReader.GetString("Categorie");
			
			return consumptie;
		}
	}
}

