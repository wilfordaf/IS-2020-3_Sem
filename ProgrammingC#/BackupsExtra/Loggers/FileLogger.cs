using System;
using System.IO;

namespace BackupsExtra.Loggers
{
    public class FileLogger : ILogger
    {
        private const string LogFileName = "log.log";
        private readonly bool _timePrefixRequired;

        public FileLogger(bool timePrefixRequired)
        {
            _timePrefixRequired = timePrefixRequired;
        }

        public void WriteLog(string message)
        {
            if (!File.Exists(LogFileName))
            {
                File.Create(LogFileName);
            }

            if (_timePrefixRequired)
            {
                message = $"{DateTime.UtcNow} {message}";
            }

            var sw = new StreamWriter(LogFileName, true);
            sw.WriteLine(message);
            sw.Close();
        }
    }
}