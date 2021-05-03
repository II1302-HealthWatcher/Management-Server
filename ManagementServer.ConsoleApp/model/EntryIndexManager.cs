using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace ManagementServer.ConsoleApp.model
{
    public class EntryIndexManager : IEntryIndexManager
    {
        private Dictionary<string, int> entryIndexDictionary;
        private string path;
        private bool dataAvailableToWrite;

        public EntryIndexManager(string path)
        {
            IFormatter binaryFormatter = new BinaryFormatter();
            Stream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);
            TimeSpan startTimeSpan = TimeSpan.FromSeconds(5);
            TimeSpan periodTimeSpan = TimeSpan.FromSeconds(5);
            this.path = path;
            this.dataAvailableToWrite = false;

            try
            {
                entryIndexDictionary = (Dictionary<string, int>)binaryFormatter.Deserialize(fileStream);
            }
            catch (SerializationException)
            {
                entryIndexDictionary = new Dictionary<string, int>();
            }

            fileStream.Close();

            var timer = new Timer((e) =>
            {
                if (this.dataAvailableToWrite)
                {
                    this.dataAvailableToWrite = false;
                    UpdateDictionaryOnDisk();
                }
            }, null, startTimeSpan, periodTimeSpan);
        }

        public int GetEntryIndexAndIncrement(string deviceID)
        {
            int currentIndex = 0;

            if (this.entryIndexDictionary.ContainsKey(deviceID))
            {
                this.entryIndexDictionary.TryGetValue(deviceID, out currentIndex);
                this.entryIndexDictionary.Remove(deviceID);
                this.entryIndexDictionary.Add(deviceID, currentIndex + 1);
                this.dataAvailableToWrite = true;
                return currentIndex;
            }
            else
            {
                this.entryIndexDictionary.Add(deviceID, currentIndex + 1);
                this.dataAvailableToWrite = true;
                return currentIndex;
            }
        }

        public bool ResetDeviceEntryIndex(string deviceID)
        {
            int currentIndex = 0;
            if (this.entryIndexDictionary.ContainsKey(deviceID))
            {
                this.entryIndexDictionary.Remove(deviceID);
                this.entryIndexDictionary.Add(deviceID, currentIndex);
                this.dataAvailableToWrite = true;
                return true;
            }
            return false;
        }

        private void UpdateDictionaryOnDisk()
        {
            IFormatter binaryFormatter = new BinaryFormatter();
            Stream stream = new FileStream(this.path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
            binaryFormatter.Serialize(stream, entryIndexDictionary);
            stream.Flush();
            stream.Close();
        }
    }
}