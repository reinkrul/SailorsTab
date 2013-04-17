using System;
using Gtk;
using SailorsTab.Repositories;
using System.Data;
using SailorsTab.Common;
using System.Globalization;
using System.Threading;

namespace SailorsTab.Tabmin
{
	class Program
	{
		public static readonly String LogFile = "sailors-tabmin.log";
		
		public static void Main (string[] args)
		{
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("nl-NL");
			ConfigurationFactory configurationFactory = new ConfigurationFactory();
			Configuration configuration = configurationFactory.Create();
			if (configuration == null)
			{
				// Newly created.
				return;
			}
			
			Application.Init();
			
			if (!authenticate())
			{
				return;
			}
			
			IDbConnection connection = new ConnectionFactory().createConnection(configuration.ConnectionString);
			
			IRepositoryFactory repositoryFactory = new RepositoryFactory(connection);
			
			new MainWindow(repositoryFactory).Show();
			
			Application.Run();
		}
		
		private static bool authenticate()
		{
			PasswordWindow window = new PasswordWindow();
			window.Show();
			window.WindowStateEvent += delegate(object o, WindowStateEventArgs args) {
				if (args.Event.NewWindowState == Gdk.WindowState.Withdrawn)
				{
					Application.Quit();
				}
			};

			Application.Run();
			
			bool result = false;
			if (!window.Cancelled)
			{
				
			}
			
			return true;
		}
	}
}

