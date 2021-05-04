namespace ManagementServer.ConsoleApp.model
{
    public interface IEntryIndexManager
    {
        public int GetEntryIndexAndIncrement(string deviceID);
        public bool ResetDeviceEntryIndex(string deviceID);
    }
}
