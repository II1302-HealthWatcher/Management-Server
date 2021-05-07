using System;

namespace ManagementServer.ConsoleApp.loghandler
{
    public class UserLogger : IExceptionLogger
    {
        /// <summary>
        /// Initializes an instance of the <see cref="UserLogger"/> class.
        /// </summary>
        public UserLogger()
        {
        }

        /// <summary>
        /// Logs an exception message to the console output stream.
        /// </summary>
        /// <param name="exceptionToLog">The exception to be logged.</param>
        public void Log(Exception exceptionToLog)
        {
            try
            {
                DateTime currentDate = DateTime.Now;
                string currentDateTimeFormatted = currentDate.ToString("yyyy-MM-dd hh:mm:ss");
                string exceptionTypeAndMessage = $"{currentDateTimeFormatted} - { exceptionToLog.GetType().Name } : { exceptionToLog.Message}";
                string logContent = exceptionTypeAndMessage + Environment.NewLine;
                Console.WriteLine("=======================================");
                Console.WriteLine("An exception has occured:");
                Console.WriteLine(logContent);
                Console.WriteLine("=======================================");
            }
            catch (Exception exception)
            {
                Console.WriteLine("=======================================");
                Console.WriteLine("The user logger has failed:");
                Console.WriteLine($"{exception.GetType().Name} : {exception.Message}");
                Console.WriteLine("=======================================");
            }
        }
    }
}