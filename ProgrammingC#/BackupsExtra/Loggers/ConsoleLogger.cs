using System;

namespace BackupsExtra.Loggers
{
    public class ConsoleLogger : ILogger
    {
        private readonly bool _timePrefixRequired;

        public ConsoleLogger(bool timePrefixRequired)
        {
            _timePrefixRequired = timePrefixRequired;
        }

        public void WriteLog(string message)
        {
            if (_timePrefixRequired)
            {
                message = $"{DateTime.UtcNow} {message}";
            }

            Console.WriteLine(message);
        }
    }
}