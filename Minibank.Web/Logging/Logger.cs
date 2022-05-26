using System;
using System.IO;

namespace Minibank.Web.Logging
{
    public class Logger : ILogger
    {
        private readonly string _fileName;

        public Logger(string fileName)
        {
            _fileName = fileName;
        }

        public void Log(Exception exception)
        {
            File.WriteAllTextAsync(_fileName, exception.Message);
        }
    }
}
