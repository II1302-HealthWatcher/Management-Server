using System;
using System.IO;

namespace ManagementServer.ConsoleApp.loghandler
{
    public class DeveloperLogger : IExceptionLogger
    {
        private string filePath;

        /// <summary>
        /// Initializes an instance of the <see cref="DeveloperLogger"/> class.
        /// </summary>
        public DeveloperLogger()
        {
            try
            {
                DateTime currentDate = DateTime.Now;
                string currentDateFormatted = currentDate.ToString("yyyy-MM-dd");
                string logFilename = $"log-{currentDateFormatted}.txt";
                this.filePath = logFilename;
            }
            catch (Exception exception)
            {
                Console.WriteLine("The developer logger failed:");
                Console.WriteLine($"{exception.GetType().Name} : {exception.Message}");
                Console.WriteLine(exception.StackTrace);
            }
        }

        /// <summary>
        /// Logs an exception with its stack trace to a file on the disk.
        /// </summary>
        /// <param name="exceptionToLog">The exception to be logged.</param>
        public void Log(Exception exceptionToLog)
        {
            try
            {
                DateTime currentDate = DateTime.Now;
                string currentDateTimeFormatted = currentDate.ToString("yyyy-MM-dd HH:mm:ss");
                string exceptionTypeAndMessage = $"{currentDateTimeFormatted} - { exceptionToLog.GetType().Name } : { exceptionToLog.Message}";
                string exceptionStackTrace = exceptionToLog.StackTrace;
                string logContent = exceptionTypeAndMessage + Environment.NewLine + exceptionStackTrace + Environment.NewLine + Environment.NewLine;
                File.AppendAllText(this.filePath, logContent);
            }
            catch (Exception exception)
            {
                Console.WriteLine("=======================================");
                Console.WriteLine("The developer logger failed:");
                Console.WriteLine($"{exception.GetType().Name} : {exception.Message}");
                Console.WriteLine(exception.StackTrace);
                Console.WriteLine("=======================================");
            }
        }
    }
}