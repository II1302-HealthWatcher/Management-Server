namespace ManagementServer.ConsoleApp.integration
{
    public interface IDecryptionServiceProvider
    {
        /// <summary>
        /// Encrypts a message using AES and encodes the output using Base64.
        /// </summary>
        /// <param name="toEncrypt">The message to be encrypted.</param>
        /// <returns>A string that holds the encrypted and Base64 encoded message.</returns>
        public string Encrypt(string toEncrypt);

        /// <summary>
        /// Decrypts an AES encrypted and Base64 encoded message.
        /// </summary>
        /// <param name="toDecrypt">The AES encrypted message encoded with Base64.</param>
        /// <returns>A string that holds the decrypted message.</returns>
        public string Decrypt(string toDecrypt);
    }
}