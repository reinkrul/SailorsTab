using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using SailorsTab.Common;

namespace SailorsTab
{
   public class HttpAgent
    {
        private string url;
        private readonly Log log = new Log(Program.LogFile, typeof(HttpAgent));

        public HttpAgent(string url)
        {
            this.url = url;
        }

        public void notify(string titel, string tekst)
        {
            if (url != null)
            {
                try
                {
                    WebClient client = new WebClient();
                    client.DownloadData(url + "?title=" + Uri.EscapeUriString(titel) + "&text=" + Uri.EscapeUriString(tekst));
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
        }
    }
}
