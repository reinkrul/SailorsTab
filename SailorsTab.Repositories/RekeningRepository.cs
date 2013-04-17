using System;
using System.Data;
using System.Collections.Generic;
using SailorsTab.Domain;
using System.Data.SQLite;

namespace SailorsTab.Repositories
{
	public class RekeningRepository : RepositoryBase, IRekeningRepository
	{
        public RekeningRepository(IDbConnection connection)
            : base(connection)
        {
        }
		
		public void Create(Rekening rekening)
		{
			if (rekening == null)
			{
				throw new ArgumentNullException("Rekening kan niet null zijn.");
			}
			
			if (FindByNaam(rekening.Naam) != null)
			{
				throw new RepositoryException("Er bestaat al een rekening met naam '" + rekening.Naam + "'.");
			}
			
			IDbCommand command = connection.CreateCommand();
			command.CommandText = "INSERT INTO rekening (naam, waarde) VALUES (@naam, @waarde)";
			command.AddParameter("@naam", rekening.Naam);
			command.AddParameter("@waarde", rekening.Waarde);
			command.Prepare();
			
			if (command.ExecuteNonQuery() != 1) {
				throw new RepositoryException("Kan rekening niet maken.");
			}

            Commit();
		}
		
		public void Update(Rekening rekening)
		{
			if (rekening.Id == 0)
			{
				throw new RepositoryException("Ongeldige rekening.");
			}
			
			IDbCommand command = connection.CreateCommand();
			command.CommandText = "UPDATE rekening SET Naam = @naam, Waarde = @waarde WHERE rowid = @id";
			command.AddParameter("@naam", rekening.Naam);
			command.AddParameter("@waarde", rekening.Waarde);
			command.AddParameter("@id", rekening.Id);
			command.Prepare();
			
			command.ExecuteNonQuery();

            Commit();
		}
		
		public Rekening[] GetAll() {
			IDbCommand command = connection.CreateCommand();
            command.CommandText = "SELECT rowid, * FROM rekening ORDER BY naam ASC";
			
			List<Rekening> rekeningen = new List<Rekening>();
			
			using (SQLiteDataReader reader = (SQLiteDataReader)command.ExecuteReader())
			{
                if (!reader.HasRows)
                {
                    return new Rekening[] {};
                }
                while (reader.Read())
                {
                    Rekening rekening = readRekening(reader);
                    rekeningen.Add(rekening);
                } 
			}
			
			return rekeningen.ToArray();
		}
		
		public Rekening FindByNaam(string naam)
		{
			IDbCommand command = connection.CreateCommand();
			command.CommandText = "SELECT rowid, * FROM rekening WHERE naam LIKE @naam";
			command.AddParameter("@naam", naam);
			command.Prepare();
			
			Rekening rekening = null;
			using (IDataReader dataReader = command.ExecuteReader())
			{
				if (dataReader.Read())
				{
					rekening = readRekening(dataReader);
				}
			}
			
			return rekening;
		}
		
		public void ConsumptieAfrekenen(Rekening rekening, Consumptie consumptie)
		{
			rekening.Waarde += consumptie.Prijs;
			IDbTransaction transaction = connection.BeginTransaction();
			try {
				save(rekening);
			
				logAfrekening(rekening, consumptie);
				transaction.Commit();
                Commit();
			}
			catch(Exception ex) {
				transaction.Rollback();
				rekening.Waarde -= consumptie.Prijs;
				throw new RepositoryException("Kan consumptie '" + consumptie.Naam + "' niet afrekenen voor '" + rekening.Naam + "'.", ex);
			}
		}
		
		public void Delete(string naam)
		{
			IDbCommand command = connection.CreateCommand();
			command.CommandText = "DELETE FROM rekening WHERE Naam LIKE @naam";
			command.AddParameter("@naam", naam);
			command.Prepare();
			
			if (command.ExecuteNonQuery() != 1)
			{
				throw new RepositoryException("Kan rekening '" + naam + "' niet verwijderen.");
			}

            Commit();
		}
		
		private void save(Rekening rekening)
		{
			if (!rekening.Validate())
			{
				throw new RepositoryException("De rekening '" + rekening.Naam + "' is niet valide.");
			}
			
			if (rekening.Id == 0)
			{
				// Bestaat nog niet, dus nieuw aanmaken
				Create(rekening);
			}
			else
			{
				// Bestaat al, dus updaten
				IDbCommand command = connection.CreateCommand();
				command.CommandText = "UPDATE rekening SET Naam = @naam, Waarde = @waarde WHERE rowid = @id";
				command.AddParameter("@id", rekening.Id);
				command.AddParameter("@naam", rekening.Naam);
				command.AddParameter("@waarde", rekening.Waarde);
				command.Prepare();
				
				if (command.ExecuteNonQuery() != 1)
				{
					throw new RepositoryException("Kan rekening '" + rekening.Naam + "' niet opslaan.");
				}
			}
		}
		
		private Rekening readRekening(IDataReader dataReader)
		{
			Rekening rekening = new Rekening();
			rekening.Id = dataReader.GetInt32("rowid");
			rekening.Naam = dataReader.GetString("Naam");
			rekening.Waarde = dataReader.GetDecimal("Waarde");
			
			return rekening;
		}
		
		private void logAfrekening(Rekening rekening, Consumptie consumptie)
		{
			IDbCommand command = connection.CreateCommand();
			command.CommandText = "INSERT INTO consumptielog (rekeningid, rekeningnaam, consumptieid, consumptienaam, datum) VALUES (@rekeningId, @rekeningNaam, @consumptieId, @consumptieNaam, current_timestamp)";
			command.AddParameter("@rekeningId", rekening.Id);
			command.AddParameter("@rekeningNaam", rekening.Naam);
			command.AddParameter("@consumptieId", consumptie.Id);
			command.AddParameter("@consumptieNaam", consumptie.Naam);
			command.Prepare();
			
			if (command.ExecuteNonQuery() != 1)
			{
				throw new RepositoryException("Kan consumptie afrekening van '" + consumptie.Naam + "' niet loggen voor rekening '" + rekening.Naam + "'.");
			}
		}
	}
}

