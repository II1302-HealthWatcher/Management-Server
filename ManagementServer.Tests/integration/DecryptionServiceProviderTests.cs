using ManagementServer.ConsoleApp.integration;
using Xunit;

namespace ManagementServer.Tests.integration
{
    public class DecryptionServiceProviderTests
    {
        [Fact]
        public void DecryptTest()
        {
            DecryptionServiceProvider decryptionServiceProvider = new DecryptionServiceProvider("DecryptTest");
            string messageToEncrypt = "This is the test message.";
            string encryptedMessage = decryptionServiceProvider.Encrypt(messageToEncrypt);
            string decryptedMessage = decryptionServiceProvider.Decrypt(encryptedMessage);

            Assert.Equal(messageToEncrypt, decryptedMessage);
        }
    }
}