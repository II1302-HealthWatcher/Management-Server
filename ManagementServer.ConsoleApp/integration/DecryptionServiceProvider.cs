﻿using System;
using System.Security.Cryptography;
using System.Text;

namespace ManagementServer.ConsoleApp.integration
{
    public class DecryptionServiceProvider : IDecryptionServiceProvider
    {
        private byte[] hashedKey;

        public DecryptionServiceProvider(string decryptionKey)
        {
            byte[] key = Encoding.UTF8.GetBytes(decryptionKey);
            SHA1CryptoServiceProvider sha1CryptoServiceProvider = new SHA1CryptoServiceProvider();
            byte[] internalHashedKey = new byte[16];
            byte[] sha1KeyHash = sha1CryptoServiceProvider.ComputeHash(key);
            Array.Copy(sha1KeyHash, internalHashedKey, 16);
            this.hashedKey = internalHashedKey;
        }

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
