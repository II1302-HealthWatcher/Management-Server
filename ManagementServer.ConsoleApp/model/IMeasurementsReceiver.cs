using ManagementServer.ConsoleApp.integration;

namespace ManagementServer.ConsoleApp.model
{
    public interface IMeasurementsReceiver
    {
        public bool StartServer(string listenPath);
        public bool StopServer();
    }
}
