using System.Net.Http;
using System.Text;

namespace ManagementServer.ConsoleApp.integration
{
    public class FirebaseClient : IFirebaseClient
    {
        private HttpClient httpClient;
        private string firebaseDbToken;
        private string realtimeDatabaseLink;

        /// <summary>
        /// Initializes an instance of the <see cref="FirebaseClient"/> class.
        /// </summary>
        /// <param name="realtimeDatabaseLink">The link to your Firebase Realtime Database.</param>
        /// <param name="firebaseDbToken">The Realtime Database Secret Token.</param>
        public FirebaseClient(string realtimeDatabaseLink, string firebaseDbToken)
        {
            this.httpClient = new HttpClient();
            this.firebaseDbToken = firebaseDbToken;
            this.realtimeDatabaseLink = realtimeDatabaseLink;
        }

        /// <summary>
        /// Posts Json data to a Firebase Realtime Database.
        /// </summary>
        /// <param name="parent">The parent entry name.</param>
        /// <param name="child">The child entry name.</param>
        /// <param name="jsonData">The Json string that holds the child data.</param>
        public void PostData(string parent, string child, string jsonData)
        {
            HttpContent requestContent = new StringContent(jsonData, Encoding.UTF8, "text/plain");
            string link = $"{this.realtimeDatabaseLink}/{parent}/{child}.json?auth={this.firebaseDbToken}";
            this.httpClient.PostAsync(link, requestContent).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Puts Json data into a Firebase Realtime Database.
        /// </summary>
        /// <param name="parent">The parent entry name.</param>
        /// <param name="child">The child entry name.</param>
        /// <param name="jsonData">The Json string that holds the child data.</param>
        public void PutData(string parent, string child, string jsonData)
        {
            HttpContent requestContent = new StringContent(jsonData, Encoding.UTF8, "text/plain");
            string link = $"{this.realtimeDatabaseLink}/{parent}/{child}.json?auth={this.firebaseDbToken}";
            this.httpClient.PutAsync(link, requestContent).GetAwaiter().GetResult();
        }
    }
}