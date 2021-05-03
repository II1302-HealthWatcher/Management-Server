using System;
using System.Collections.Generic;
using System.Text;

namespace ManagementServer.ConsoleApp.integration
{
    public interface IFirebaseClient
    {
        public string PostData(string parent, string child, string jsonData);
        public string PutData(string parent, string child, string jsonData);
    }
}
