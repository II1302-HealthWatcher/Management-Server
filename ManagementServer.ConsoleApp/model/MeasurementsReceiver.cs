using ManagementServer.ConsoleApp.dto;
using ManagementServer.ConsoleApp.integration;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace ManagementServer.ConsoleApp.model
{
    public class MeasurementsReceiver : IMeasurementsReceiver
    {
        private IFirebaseClient firebaseClient;
        private IDecryptionServiceProvider decryptionServiceProvider;
        private IEntryIndexManager entryIndexManager;
        private HttpListener httpListener;

        public MeasurementsReceiver(IFirebaseClient firebaseClient, IDecryptionServiceProvider decryptionServiceProvider, IEntryIndexManager entryIndexManager)
        {
            this.firebaseClient = firebaseClient;
            this.decryptionServiceProvider = decryptionServiceProvider;
            this.entryIndexManager = entryIndexManager;
        }

        public bool StartServer(string listenPath)
        {
            this.httpListener = new HttpListener();
            this.httpListener.Prefixes.Add(listenPath);
            this.httpListener.Start();
            this.httpListener.BeginGetContext(new AsyncCallback(ListenerCallback), httpListener);
            return true;
        }

        public bool StopServer()
        {
            this.httpListener.Stop();
            return true;
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

        private bool processMeasurments(string encryptedBody)
        {
            string decryptedBody = processEncryptedResponse(encryptedBody);
            MeasurementsDTO measurementsDTO = new MeasurementsDTO();
            string deviceID = "";
            // TODO: Fill Measurements DTO and extract deviceID
            SendMeasurmentsToDatabase(measurementsDTO, deviceID);
            return true;
        }

        private string processEncryptedResponse(string encryptedData)
        {
            string decryptedData = this.decryptionServiceProvider.Decrypt(encryptedData);
            return decryptedData;
        }

        private bool SendMeasurmentsToDatabase(MeasurementsDTO measurementsDTO, string deviceID)
        {
            string jsonData = measurementsDTO.ToJson();
            string entryIndex = this.entryIndexManager.GetEntryIndexAndIncrement(deviceID).ToString();
            this.firebaseClient.PutData($"users\\{deviceID}\\measurementsList", entryIndex, jsonData);
            return true;
        }
    }
}