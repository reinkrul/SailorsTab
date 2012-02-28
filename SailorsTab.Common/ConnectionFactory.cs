using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SailorsTab.Common
{
    public class ConfigurationFactory
    {
        // Fields
        private const string CONFIGURATION_FILENAME = "configuration.xml";

        public Configuration Create()
        {
            Configuration configuration1 = null;
            try
            {
                using (Stream stream = File.Open(CONFIGURATION_FILENAME, FileMode.Open))
                {
                    configuration1 = Configuration.Load(stream);
                }

            }
            catch (Exception exception)
            {
                Console.WriteLine("Default configuration created: " + exception);
                using (Stream stream = File.Open(CONFIGURATION_FILENAME, FileMode.Create))
                {
                    new Configuration().Save(stream);
                }
            }
            return configuration1;
        }
    }
}
