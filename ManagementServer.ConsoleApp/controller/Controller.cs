using ManagementServer.ConsoleApp.integration;
using ManagementServer.ConsoleApp.loghandler;
using ManagementServer.ConsoleApp.model;

namespace ManagementServer.ConsoleApp.controller
{
    public class Controller
    {
        private IMeasurementsReceiver measurementsReceiver;

        public Controller(string EIMDictionaryPath, IFirebaseClient firebaseClient, IDecryptionServiceProvider decryptionServiceProvider, IExceptionLogger devLogger)
        {
            IEntryIndexManager entryIndexManager = new EntryIndexManager(EIMDictionaryPath);
            this.measurementsReceiver = new MeasurementsReceiver(firebaseClient, decryptionServiceProvider, entryIndexManager, devLogger);
        }

        public void StartServer(string listenPath)
        {
            this.measurementsReceiver.StartServer(listenPath);
        }

        public void SetVerbosity(bool verbose)
        {
            this.measurementsReceiver.SetVerbosity(verbose);
        }

        public void StopServer()
        {
            this.measurementsReceiver.StopServer();
        }
    }
}