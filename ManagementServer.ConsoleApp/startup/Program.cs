using ManagementServer.ConsoleApp.integration;
using ManagementServer.ConsoleApp.model;
using System;

namespace ManagementServer.ConsoleApp
{
    public class Program
    {
        /// <summary>
        /// A dummy method to be used with the xUnit tests.
        /// </summary>
        /// <returns>A boolean that holds the value true.</returns>
        public bool dummy()
        {
            return true;
        }

        /// <summary>
        /// The main method that initializes the needed instances and starts the HealthWatcher Management Server.
        /// </summary>
        /// <param name="args">The program expects no command line arguments.</param>
        static void Main(string[] args)
        {
            string firebaseDbToken = "KXdQfwlci2Cytgg8tOJzUAJA0zgK9tflQ5Qit720";
            string realtimeDatabaseLink = "https://healthwatcher-f04bf-default-rtdb.firebaseio.com";
            string decryptionKey = "HealthWatcherKey";
            string EIMDictionaryPath = "EIMDictionary.bin";

            IEntryIndexManager entryIndexManager = new EntryIndexManager(EIMDictionaryPath);
            IDecryptionServiceProvider decryptionServiceProvider = new DecryptionServiceProvider(decryptionKey);
            IFirebaseClient firebaseClient = new FirebaseClient(realtimeDatabaseLink, firebaseDbToken);
            IMeasurementsReceiver measurementsReceiver = new MeasurementsReceiver(firebaseClient, decryptionServiceProvider, entryIndexManager);
            measurementsReceiver.StartServer("http://192.168.0.106:1234/");
            measurementsReceiver.SetVerbosity(true);
            Console.ReadLine();
            measurementsReceiver.StopServer();
        }
    }
}
