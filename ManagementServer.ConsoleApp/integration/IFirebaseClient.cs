namespace ManagementServer.ConsoleApp.integration
{
    public interface IFirebaseClient
    {
        /// <summary>
        /// Posts Json data to a Firebase Realtime Database.
        /// </summary>
        /// <param name="parent">The parent entry name.</param>
        /// <param name="child">The child entry name.</param>
        /// <param name="jsonData">The Json string that holds the child data.</param>
        public void PostData(string parent, string child, string jsonData);

        /// <summary>
        /// Puts Json data into a Firebase Realtime Database.
        /// </summary>
        /// <param name="parent">The parent entry name.</param>
        /// <param name="child">The child entry name.</param>
        /// <param name="jsonData">The Json string that holds the child data.</param>
        public void PutData(string parent, string child, string jsonData);
    }
}
