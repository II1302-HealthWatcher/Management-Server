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

        /// <summary>
        /// Initializes an instance of the <see cref="EntryIndexManager"/> class.
        /// </summary>
        /// <param name="path">The path to the EIM dictionary file.</param>
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

        /// <summary>
        /// Gets the entry index to be used with a Firebase Realtime Database entry and increments the index of the specified device.
        /// </summary>
        /// <param name="deviceID">The HealthWatcher device identifier.</param>
        /// <returns>An integer that holds the entry index for the specified device identifier.</returns>
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

        /// <summary>
        /// Resets the entry index of the specified device.
        /// </summary>
        /// <param name="deviceID">The HealthWatcher device identifier.</param>
        /// <returns>A boolean indicating whether the device identifier was found and reset in the EIM dictionary or not.</returns>
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