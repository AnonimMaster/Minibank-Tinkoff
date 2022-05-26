using System;

namespace Minibank.Web.Logging
{
    public interface ILogger
    {
        public void Log(Exception exception);
    }
}
