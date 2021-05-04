using System.Net.Http;
using System.Text;

namespace ManagementServer.ConsoleApp.integration
{
    public class FirebaseClient : IFirebaseClient
    {
        private HttpClient httpClient;
        private string firebaseDbToken;
        private string realtimeDatabaseLink;

        public FirebaseClient(string realtimeDatabaseLink, string firebaseDbToken)
        {
            this.httpClient = new HttpClient();
            this.firebaseDbToken = firebaseDbToken;
            this.realtimeDatabaseLink = realtimeDatabaseLink;
        }

        public void PostData(string parent, string child, string jsonData)
        {
            HttpContent requestContent = new StringContent(jsonData, Encoding.UTF8, "text/plain");
            string link = $"{this.realtimeDatabaseLink}/{parent}/{child}.json?auth={this.firebaseDbToken}";
            this.httpClient.PostAsync(link, requestContent).GetAwaiter().GetResult();
        }

        public void PutData(string parent, string child, string jsonData)
        {
            HttpContent requestContent = new StringContent(jsonData, Encoding.UTF8, "text/plain");
            string link = $"{this.realtimeDatabaseLink}/{parent}/{child}.json?auth={this.firebaseDbToken}";
            this.httpClient.PutAsync(link, requestContent).GetAwaiter().GetResult();
        }
    }
}
