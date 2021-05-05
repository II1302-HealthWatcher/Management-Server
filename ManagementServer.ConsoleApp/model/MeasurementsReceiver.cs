using ManagementServer.ConsoleApp.dto;
using ManagementServer.ConsoleApp.integration;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace ManagementServer.ConsoleApp.model
{
    public class MeasurementsReceiver : IMeasurementsReceiver
    {
        private IFirebaseClient firebaseClient;
        private IDecryptionServiceProvider decryptionServiceProvider;
        private IEntryIndexManager entryIndexManager;
        private HttpListener httpListener;
        private bool verbose;

        /// <summary>
        /// Initializes an instance of the <see cref="MeasurementsReceiver"/> class.
        /// </summary>
        /// <param name="firebaseClient">An instance of a class that implements the <see cref="IFirebaseClient"/> interface.</param>
        /// <param name="decryptionServiceProvider">An instance of a class that implements the <see cref="IDecryptionServiceProvider"/> interface.</param>
        /// <param name="entryIndexManager">An instance of a class that implements the <see cref="IEntryIndexManager"/> interface.</param>
        public MeasurementsReceiver(IFirebaseClient firebaseClient, IDecryptionServiceProvider decryptionServiceProvider, IEntryIndexManager entryIndexManager)
        {
            this.firebaseClient = firebaseClient;
            this.decryptionServiceProvider = decryptionServiceProvider;
            this.entryIndexManager = entryIndexManager;
        }

        /// <summary>
        /// Starts the HTTP server which will receive the measurements from the HealthWatcher device simulator.
        /// </summary>
        /// <param name="listenPath">The fully qualified listen path following the format "http://IP:PORT/".</param>
        public void StartServer(string listenPath)
        {
            this.httpListener = new HttpListener();
            this.httpListener.Prefixes.Add(listenPath);
            this.httpListener.Start();
            this.httpListener.BeginGetContext(new AsyncCallback(ListenerCallback), httpListener);
        }

        /// <summary>
        /// Stops the HTTP server which is receiving the measurements from the HealthWatcher device simulator.
        /// </summary>
        public void StopServer()
        {
            this.httpListener.Stop();
        }

        /// <summary>
        /// Sets the verbosity of the measurements receiver.
        /// </summary>
        /// <param name="verbose">A boolean indicating whether to print out the received data from the HealthWatcher device simulator or not.</param>
        public void SetVerbosity(bool verbose)
        {
            this.verbose = verbose;
        }

        private void ListenerCallback(IAsyncResult result)
        {
            HttpListener httpListener = (HttpListener)result.AsyncState;
            HttpListenerContext context = httpListener.EndGetContext(result);
            httpListener.BeginGetContext(new AsyncCallback(ListenerCallback), httpListener);
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;
            Stream requestBodyStream = request.InputStream;
            Encoding requestEncoding = request.ContentEncoding;
            StreamReader requestBodyReader = new StreamReader(requestBodyStream, requestEncoding);
            string requestBody = requestBodyReader.ReadToEnd();
            processMeasurments(requestBody);
            string responseString = "OK";
            byte[] buffer = Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            Stream outputStream = response.OutputStream;
            outputStream.Write(buffer, 0, buffer.Length);
            outputStream.Close();
        }

        private void processMeasurments(string encryptedBody)
        {
            string decryptedBody = processEncryptedResponse(encryptedBody);
            string deviceID = extractDeviceID(decryptedBody);
            MeasurementsDTO measurementsDTO =  generateMeasurementsDTO(decryptedBody);
            if(this.verbose)
            {
                PrintLog(decryptedBody);
            }
            SendMeasurmentsToDatabase(measurementsDTO, deviceID);
        }


        private void PrintLog(string measurementsData)
        {
            Console.WriteLine("=======================================");
            Console.WriteLine(measurementsData);
            Console.WriteLine("=======================================");
            Console.WriteLine();
        }

        private string extractDeviceID(string input)
        {
            string pattern = "Device ID: (.*?)" + "\r\n";
            RegexOptions options = RegexOptions.Multiline;
            string[] deviceIDDataArray = Regex.Split(input, pattern, options);
            string deviceID = deviceIDDataArray[1];
            return deviceID;
        }

        private MeasurementsDTO generateMeasurementsDTO(string input)
        {
            string pattern = "Measurement Date: (.*?)" + "\r\n"
                + "Body temperature: (.*?)" + "\r\n"  
                + "Blood Oxygen Level: (.*?)" + "\r\n" 
                + "Heart Pulse Rate: (.*?)" + "\r\n";
            RegexOptions options = RegexOptions.Multiline;
            string[] measurementsDataArray = Regex.Split(input, pattern, options);

            MeasurementsDTO measurementsDTO = new MeasurementsDTO();
            measurementsDTO.MeasurementDate = measurementsDataArray[1];
            measurementsDTO.BodyTemperature = measurementsDataArray[2];
            measurementsDTO.BloodOxygenLevel = measurementsDataArray[3];
            measurementsDTO.HeartPulse = measurementsDataArray[4];

            return measurementsDTO;
        }

        private string processEncryptedResponse(string encryptedData)
        {
            string decryptedData = this.decryptionServiceProvider.Decrypt(encryptedData);
            return decryptedData;
        }

        private void SendMeasurmentsToDatabase(MeasurementsDTO measurementsDTO, string deviceID)
        {
            string jsonData = measurementsDTO.ToJson();
            string entryIndex = this.entryIndexManager.GetEntryIndexAndIncrement(deviceID).ToString();
            this.firebaseClient.PutData($"users\\{deviceID}\\measurementsList", entryIndex, jsonData);
        }
    }
}