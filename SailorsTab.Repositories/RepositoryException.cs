using System;

namespace SailorsTab.Repositories
{
	public class RepositoryException : Exception
	{
		public RepositoryException(String message, Exception innerException) : base(message, innerException) {
		
		}
		
		public RepositoryException(String message) : base(message) {
		
		}
	}
}

