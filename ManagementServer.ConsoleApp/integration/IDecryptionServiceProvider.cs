using System;
using System.Collections.Generic;
using System.Text;

namespace ManagementServer.ConsoleApp.integration
{
    public interface IDecryptionServiceProvider
    {
        public string Decrypt(string toDecrypt);
    }
}
