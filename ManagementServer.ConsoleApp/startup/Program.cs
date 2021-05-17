using ManagementServer.ConsoleApp.controller;
using ManagementServer.ConsoleApp.integration;
using ManagementServer.ConsoleApp.loghandler;
using ManagementServer.ConsoleApp.view;

namespace ManagementServer.ConsoleApp
{
    public class Program
    {
        /// <summary>
        /// A dummy method to be used with the xUnit tests.
        /// </summary>
        /// <returns>A boolean that holds the value true.</returns>
        public bool Dummy()
        {
            return true;
        }

        /// <summary>
        /// The main method that initializes the needed instances and starts the HealthWatcher Management Server.
        /// </summary>
        /// <param name="args">The program expects no command line arguments.</param>
        private static void Main(string[] args)
        {
            string firebaseDbToken = "KXdQfwlci2Cytgg8tOJzUAJA0zgK9tflQ5Qit720";
            string realtimeDatabaseLink = "https://healthwatcher-f04bf-default-rtdb.firebaseio.com";
            string decryptionKey = "HealthWatcherKey";
            string EIMDictionaryPath = "EIMDictionary.bin";

            IDecryptionServiceProvider decryptionServiceProvider = new DecryptionServiceProvider(decryptionKey);
            IFirebaseClient firebaseClient = new FirebaseClient(realtimeDatabaseLink, firebaseDbToken);
            IExceptionLogger devLogger = new DeveloperLogger();
            IExceptionLogger userLogger = new UserLogger();
            Controller controller = new Controller(EIMDictionaryPath, firebaseClient, decryptionServiceProvider, devLogger);
            MainView mainView = new MainView(controller, userLogger);
            mainView.StartView();
        }
    }
}