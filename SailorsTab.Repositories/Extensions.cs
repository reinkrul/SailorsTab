using System;
using System.Data;

namespace SailorsTab.Repositories
{
	public static class Extensions
	{
		public static int GetInt32(this IDataReader dataReader, String name)
		{
			int ordinal = dataReader.GetOrdinal(name);
			return dataReader.GetInt32(ordinal);
		}
		
		public static string GetString(this IDataReader dataReader, String name)
		{
			int ordinal = dataReader.GetOrdinal(name);	
			return dataReader.IsDBNull(ordinal) ? null : dataReader.GetString(ordinal);
		}
		
		public static Decimal GetDecimal(this IDataReader dataReader, String name)
		{
			int ordinal = dataReader.GetOrdinal(name);
			return dataReader.GetDecimal(ordinal);
		}
		
		public static void AddParameter(this IDbCommand command, string name, object value)
		{
			IDbDataParameter dataParameter = command.CreateParameter();
			dataParameter.ParameterName = name;
			dataParameter.Value = value;
			command.Parameters.Add(dataParameter);
		}
	}
}

