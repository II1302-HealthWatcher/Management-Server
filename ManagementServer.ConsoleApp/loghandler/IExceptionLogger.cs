using System;

namespace ManagementServer.ConsoleApp.loghandler
{
    public interface IExceptionLogger
    {
        /// <summary>
        /// Logs an exception message.
        /// </summary>
        /// <param name="exceptionToLog">The exception to be logged.</param>
        public void Log(Exception exceptionToLog);
    }
}