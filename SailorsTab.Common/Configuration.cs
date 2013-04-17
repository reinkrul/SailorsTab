using System;
using System.IO;
using System.Xml.Serialization;

namespace SailorsTab.Common
{
	public class Configuration
	{
		public static Configuration Load(Stream stream)
		{
            XmlSerializer serializer = new XmlSerializer(typeof(Configuration));
            return (Configuration)serializer.Deserialize(stream);
		}

        public void Save(Stream stream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Configuration));
            serializer.Serialize(stream, this);
        }
		
		public String ConnectionString { get; set; }
		public String Password { get; set; }
        public String AgentUrl { get; set; }
	}
}

