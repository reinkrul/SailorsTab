using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SailorsTab.Repositories
{
    public class RepositoryBase
    {
        protected IDbConnection connection;

        public RepositoryBase(IDbConnection connection)
        {
			this.connection = connection;
		}

        protected void Commit()
        {
            // Is called after every functional update to the database.
            // For future use, we could 'flush' the database connection if
            // data loss is occuring due to crashes or power failures.
        }
    }
}
