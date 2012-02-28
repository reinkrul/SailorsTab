using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SailorsTab.Repositories
{
    public class BackupService
    {
        private string targetDirectory = "backups";

        public BackupService()
        {
            initialize();
        }
        public BackupService(string directory)
        {
            this.targetDirectory = directory;
            initialize();
        }

        private void initialize()
        {
            if (!Directory.Exists(targetDirectory))
            {
                Directory.CreateDirectory(targetDirectory);
            }
        }
        public void Backup(string fileName)
        {
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException("Source file for backup not found: " + fileName);
            }

            File.Copy(fileName, Path.Combine(targetDirectory, getBackupFileName(Path.GetFileName(fileName))));
        }

        private string getBackupFileName(string fileName)
        {
            string timestamp = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString()
                + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();

            int index = fileName.LastIndexOf('.');
            if (index == -1)
            {
                return fileName + "-" + timestamp;
            }
            else
            {
                return fileName.Substring(0, index) + "-" + timestamp + "." + fileName.Substring(index + 1);
            }
        }
    }
}
