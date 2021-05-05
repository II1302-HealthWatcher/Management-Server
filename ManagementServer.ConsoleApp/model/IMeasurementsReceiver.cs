namespace ManagementServer.ConsoleApp.model
{
    public interface IMeasurementsReceiver
    {
        /// <summary>
        /// Starts the HTTP server which will receive the measurements from the HealthWatcher device simulator.
        /// </summary>
        /// <param name="listenPath">The fully qualified listen path following the format "http://IP:PORT/".</param>
        public void StartServer(string listenPath);

        /// <summary>
        /// Stops the HTTP server which is receiving the measurements from the HealthWatcher device simulator.
        /// </summary>
        public void StopServer();

        /// <summary>
        /// Sets the verbosity of the measurements receiver.
        /// </summary>
        /// <param name="verbose">A boolean indicating whether to print out the received data from the HealthWatcher device simulator or not.</param>
        public void SetVerbosity(bool verbose);
    }
}
