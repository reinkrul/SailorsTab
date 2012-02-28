using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SailorsTab.Common
{
    public class Log
    {
        // Fields
        private static readonly object lockObject;
        private Type loggingType;
        private string logFile;

        static Log()
        {
            lockObject = new object();
        }

        public Log(string logFile, Type loggingType)
        {
            this.loggingType = loggingType;
            this.logFile = logFile;
        }

        public void Error(Exception ex)
        {
            string str1 = this.formatMessage("error", ex.ToString());
            System.Threading.Monitor.Enter(lockObject);
            try
            {
                try
                {
                    System.IO.TextWriter textwriter1 = new System.IO.StreamWriter(this.logFile, true);
                    try
                    {
                        textwriter1.WriteLine(str1);
                    }
                    finally
                    {
                        if (textwriter1 != null)
                        {
                            textwriter1.Dispose();
                        }
                    }
                }
                catch (Exception exception)
                {
                    Exception exception1 = exception;
                    Console.WriteLine("ERROR: Cannot write log to " + this.logFile + "." + exception1.ToString());
                }
            }
            finally
            {
                System.Threading.Monitor.Exit(lockObject);
            }
        }

        private string formatMessage(string level, string message)
        {
            string msg = "[{0}-{1}-{2} {3}:{4}:{5}] {6} {7} - {8}";
            return string.Format(msg, DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, level.ToUpper(), loggingType.Name, message);
        }
    }
}