namespace ManagementServer.ConsoleApp.integration
{
    public interface IDecryptionServiceProvider
    {
        /// <summary>
        /// Decrypts an AES encrypted and Base64 encoded message.
        /// </summary>
        /// <param name="toDecrypt">The AES encrypted message encoded with Base64.</param>
        /// <returns>A string that holds the decrypted message.</returns>
        public string Decrypt(string toDecrypt);
    }
}
