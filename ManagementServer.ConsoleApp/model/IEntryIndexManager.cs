namespace ManagementServer.ConsoleApp.model
{
    public interface IEntryIndexManager
    {
        /// <summary>
        /// Gets the entry index to be used with a Firebase Realtime Database entry and increments the index of the specified device.
        /// </summary>
        /// <param name="deviceID">The HealthWatcher device identifier.</param>
        /// <returns>An integer that holds the entry index for the specified device identifier.</returns>
        public int GetEntryIndexAndIncrement(string deviceID);

        /// <summary>
        /// Resets the entry index of the specified device.
        /// </summary>
        /// <param name="deviceID">The HealthWatcher device identifier.</param>
        /// <returns>A boolean indicating whether the device identifier was found and reset in the EIM dictionary or not.</returns>
        public bool ResetDeviceEntryIndex(string deviceID);
    }
}
