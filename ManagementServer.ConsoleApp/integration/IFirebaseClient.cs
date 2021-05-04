namespace ManagementServer.ConsoleApp.integration
{
    public interface IFirebaseClient
    {
        public void PostData(string parent, string child, string jsonData);
        public void PutData(string parent, string child, string jsonData);
    }
}
