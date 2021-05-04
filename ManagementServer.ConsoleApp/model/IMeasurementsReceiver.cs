namespace ManagementServer.ConsoleApp.model
{
    public interface IMeasurementsReceiver
    {
        public void StartServer(string listenPath);
        public void SetVerbosity(bool verbose);
        public void StopServer();

    }
}
