using ManagementServer.ConsoleApp.integration;
using ManagementServer.ConsoleApp.model;
using System;

namespace ManagementServer.ConsoleApp
{
    public class Program
    {
        public int dummy()
        {
            return 1;
        }

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
            Console.ReadLine();
            measurementsReceiver.StopServer();
        }
    }
}
