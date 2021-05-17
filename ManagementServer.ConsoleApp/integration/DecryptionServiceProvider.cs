using System;
using System.Security.Cryptography;
using System.Text;

namespace ManagementServer.ConsoleApp.integration
{
    public class DecryptionServiceProvider : IDecryptionServiceProvider
    {
        private byte[] hashedKey;

        /// <summary>
        /// Initializes an instance of the <see cref="DecryptionServiceProvider"/> class.
        /// </summary>
        /// <param name="decryptionKey">The decryption key that will be used to decrypt encrypted messages.</param>
        public DecryptionServiceProvider(string decryptionKey)
        {
            byte[] key = Encoding.UTF8.GetBytes(decryptionKey);
            SHA1CryptoServiceProvider sha1CryptoServiceProvider = new SHA1CryptoServiceProvider();
            byte[] internalHashedKey = new byte[16];
            byte[] sha1KeyHash = sha1CryptoServiceProvider.ComputeHash(key);
            Array.Copy(sha1KeyHash, internalHashedKey, 16);
            this.hashedKey = internalHashedKey;
        }

        /// <summary>
        /// Encrypts a message using AES and encodes the output using Base64.
        /// </summary>
        /// <param name="toEncrypt">The message to be encrypted.</param>
        /// <returns>A string that holds the encrypted and Base64 encoded message.</returns>
        public string Encrypt(string toEncrypt)
        {
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);

            RijndaelManaged aesEncryptionProvider = new RijndaelManaged
            {
                KeySize = (this.hashedKey.Length * 8),
                Key = this.hashedKey,
                IV = this.hashedKey,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7
            };

            ICryptoTransform aesEncryptor = aesEncryptionProvider.CreateEncryptor();
            byte[] encryptedArray = aesEncryptor.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(encryptedArray);
        }

        /// <summary>
        /// Decrypts an AES encrypted and Base64 encoded message.
        /// </summary>
        /// <param name="toDecrypt">The AES encrypted message encoded with Base64.</param>
        /// <returns>A string that holds the decrypted message.</returns>
        public string Decrypt(string toDecrypt)
        {
            byte[] toDecryptArray = Convert.FromBase64String(toDecrypt);
            RijndaelManaged aesDecryptionProvider = new RijndaelManaged
            {
                KeySize = (this.hashedKey.Length * 8),
                Key = this.hashedKey,
                IV = this.hashedKey,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7
            };

            ICryptoTransform aesDecryptor = aesDecryptionProvider.CreateDecryptor();
            byte[] decryptedArray = aesDecryptor.TransformFinalBlock(toDecryptArray, 0, toDecryptArray.Length);
            return Encoding.UTF8.GetString(decryptedArray);
        }

        
    }
}
