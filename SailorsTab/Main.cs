using System;
using System.Data;
using SailorsTab.Domain;
using SailorsTab.Windows;
using SailorsTab.Repositories;
using SailorsTab.Common;
using Gtk;
using System.Threading;
using System.Globalization;

namespace SailorsTab
{
	class Program
	{
        private static readonly Log log = new Log(Program.LogFile, typeof(Program));
		public static readonly string LogFile = "sailors-tab.log";
        private static readonly string DatabaseFile = "sailorsbar.db";
		
		public static void Main (string[] args)
		{
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("nl-NL");
            try
            {
                new BackupService().Backup(DatabaseFile);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

			ConfigurationFactory configurationFactory = new ConfigurationFactory();
			Configuration configuration = configurationFactory.Create();
			if (configuration == null)
			{
				// Newly created.
				return;
			}
			
			IDbConnection connection = new ConnectionFactory().createConnection(configuration.ConnectionString);
			
			IRepositoryFactory repositoryFactory = new RepositoryFactory(connection);
			
			Application.Init();
			
			new MainWindow(repositoryFactory).Show();
			
			Application.Run();
		}
	}
}

