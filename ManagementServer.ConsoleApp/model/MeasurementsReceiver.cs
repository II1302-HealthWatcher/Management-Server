using ManagementServer.ConsoleApp.dto;
using ManagementServer.ConsoleApp.integration;
using ManagementServer.ConsoleApp.loghandler;
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
        private IExceptionLogger devLogger;
        private HttpListener httpListener;
        private string userAgent;
        private bool verbose;

        /// <summary>
        /// Initializes an instance of the <see cref="MeasurementsReceiver"/> class.
        /// </summary>
        /// <param name="firebaseClient">An instance of a class that implements the <see cref="IFirebaseClient"/> interface.</param>
        /// <param name="decryptionServiceProvider">An instance of a class that implements the <see cref="IDecryptionServiceProvider"/> interface.</param>
        /// <param name="entryIndexManager">An instance of a class that implements the <see cref="IEntryIndexManager"/> interface.</param>
        /// <param name="devLogger">An instance of a class that implements the <see cref="IExceptionLogger"/> interface.</param>
        public MeasurementsReceiver(IFirebaseClient firebaseClient, IDecryptionServiceProvider decryptionServiceProvider, IEntryIndexManager entryIndexManager, IExceptionLogger devLogger)
        {
            this.firebaseClient = firebaseClient;
            this.decryptionServiceProvider = decryptionServiceProvider;
            this.entryIndexManager = entryIndexManager;
            this.devLogger = devLogger;
            this.verbose = false;
            this.userAgent = "HealthWatcher / 1.0";
        }

        /// <summary>
        /// Starts the HTTP server which will receive the measurements from the HealthWatcher device simulator.
        /// </summary>
        /// <param name="listenPath">The fully qualified listen path following the format "http://IP:PORT/".</param>
        public void StartServer(string listenPath)
        {
            try
            {
                this.httpListener = new HttpListener();
                this.httpListener.Prefixes.Add(listenPath);
                this.httpListener.Start();
                this.httpListener.BeginGetContext(new AsyncCallback(ListenerCallback), httpListener);
            }
            catch (Exception exception)
            {
                this.devLogger.Log(exception);
                throw exception;
            }
        }

        /// <summary>
        /// Stops the HTTP server which is receiving the measurements from the HealthWatcher device simulator.
        /// </summary>
        public void StopServer()
        {
            try
            {
                this.httpListener.Stop();
            }
            catch (Exception exception)
            {
                this.devLogger.Log(exception);
                throw exception;
            }
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
            try
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
                string userAgent = request.Headers.GetValues("User-Agent")[0];
                if (validateUseragent(userAgent))
                {
                    if(processMeasurments(requestBody))
                    {
                        reportValidRequest(requestBody, response);
                    }
                    else
                    {
                        reportInvalidRequest("Invalid request fields!", response);
                        throw new Exception("Invalid request fields!");
                    }

                }
                else
                {
                    reportInvalidRequest("Invalid User-Agent!", response);
                    throw new Exception("Invalid User-Agent!");
                }
            }
            catch (Exception exception)
            {
                this.devLogger.Log(exception);
            }
        }

        private void reportValidRequest(string requestBody, HttpListenerResponse response)
        {
            string responseString = "OK";
            byte[] buffer = Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            Stream outputStream = response.OutputStream;
            outputStream.Write(buffer, 0, buffer.Length);
            outputStream.Close();
        }

        private void reportInvalidRequest(string message, HttpListenerResponse response)
        {
            string responseString = message;
            byte[] buffer = Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            Stream outputStream = response.OutputStream;
            outputStream.Write(buffer, 0, buffer.Length);
            outputStream.Close();
        }

        private bool validateUseragent(string userAgent)
        {
            if (userAgent.Equals(this.userAgent))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool validateRequestFields(string[] measurementsDataArray)
        {
            if (measurementsDataArray.Length == 6)
            {
                return true;
            }
            return true;
        }

        private bool processMeasurments(string encryptedBody)
        {
            try
            {
                string decryptedBody = processEncryptedResponse(encryptedBody);
                string deviceID = extractDeviceID(decryptedBody);
                MeasurementsDTO measurementsDTO = generateMeasurementsDTO(decryptedBody);
                if(measurementsDTO == null)
                {
                    return false;
                }
                if (this.verbose)
                {
                    PrintLog(decryptedBody);
                }
                SendMeasurmentsToDatabase(measurementsDTO, deviceID);
                return true;
            }
            catch (Exception exception)
            {
                this.devLogger.Log(exception);
                return false;
            }
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
            try
            {
                string pattern = "Device ID: (.*?)" + "\r\n";
                RegexOptions options = RegexOptions.Multiline;
                string[] deviceIDDataArray = Regex.Split(input, pattern, options);
                string deviceID = deviceIDDataArray[1];
                return deviceID;
            }
            catch (Exception exception)
            {
                this.devLogger.Log(exception);
                return null;
            }
        }

        private MeasurementsDTO generateMeasurementsDTO(string input)
        {
            try
            {
                string pattern = "Measurement Date: (.*?)" + "\r\n"
                + "Body temperature: (.*?)" + "\r\n"
                + "Blood Oxygen Level: (.*?)" + "\r\n"
                + "Heart Pulse Rate: (.*?)" + "\r\n";
                RegexOptions options = RegexOptions.Multiline;
                string[] measurementsDataArray = Regex.Split(input, pattern, options);
                if (validateRequestFields(measurementsDataArray))
                {
                    MeasurementsDTO measurementsDTO = new MeasurementsDTO();
                    measurementsDTO.MeasurementDate = measurementsDataArray[1];
                    measurementsDTO.BodyTemperature = measurementsDataArray[2];
                    measurementsDTO.BloodOxygenLevel = measurementsDataArray[3];
                    measurementsDTO.HeartPulse = measurementsDataArray[4];
                    return measurementsDTO;
                }

                return null;
            }
            catch (Exception exception)
            {
                this.devLogger.Log(exception);
                return null;
            }
        }

        private string processEncryptedResponse(string encryptedData)
        {
            try
            {
                string decryptedData = this.decryptionServiceProvider.Decrypt(encryptedData);
                return decryptedData;
            }
            catch (Exception exception)
            {
                this.devLogger.Log(exception);
                return null;
            }
        }

        private void SendMeasurmentsToDatabase(MeasurementsDTO measurementsDTO, string deviceID)
        {
            try
            {
                if (measurementsDTO != null && deviceID != null)
                {
                    string jsonData = measurementsDTO.ToJson();
                    string entryIndex = this.entryIndexManager.GetEntryIndexAndIncrement(deviceID).ToString();
                    this.firebaseClient.PutData($"users\\{deviceID}\\measurementsList", entryIndex, jsonData);
                }
                else
                {
                    throw new Exception("Invalid measurementsDTO or deviceID.");
                }
            }
            catch (Exception exception)
            {
                this.devLogger.Log(exception);
            }
        }
    }
}