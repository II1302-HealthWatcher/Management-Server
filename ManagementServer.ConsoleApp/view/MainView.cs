using ManagementServer.ConsoleApp.controller;
using ManagementServer.ConsoleApp.loghandler;
using System;

namespace ManagementServer.ConsoleApp.view
{
    public class MainView
    {
        private Controller controller;
        private IExceptionLogger userLogger;

        public MainView(Controller controller, IExceptionLogger userLogger)
        {
            this.controller = controller;
            this.userLogger = userLogger;
        }

        public void StartView()
        {
            Console.WriteLine("***************************************");
            Console.WriteLine("*** HealthWatcher Management Server ***");
            Console.WriteLine("***************************************");
            Console.WriteLine();
            printMenu();
        }

        private void printMenu()
        {
            Console.WriteLine("Please enter the number of the desired action:");
            Console.WriteLine("1. Start the management server");
            Console.WriteLine("2. Make the management server print the data received from the sensor device simulation");
            Console.WriteLine("3. Exit the program");
            parseUserInput();
        }

        private void parseUserInput()
        {
            try
            {
                string input = Console.ReadLine();
                int userInput = int.Parse(input);
                processUserInput(userInput);
            }
            catch (Exception)
            {
                Console.WriteLine("Malformed user input, please try again!");
                printMenu();
            }
        }

        private void processUserInput(int input)
        {
            try
            {
                switch (input)
                {
                    case 1:
                        StartServer();
                        break;

                    case 2:
                        SetVerbosity();
                        break;

                    case 3:
                        Console.WriteLine("Exiting Program!");
                        break;

                    default:
                        Console.WriteLine("Malformed user input, please try again!");
                        printMenu();
                        break;
                }
            }
            catch (Exception exception)
            {
                this.userLogger.Log(exception);
            }
        }

        private void StartServer()
        {
            Console.WriteLine("Please enter the fully qualified listen path following the format http://IP:PORT/");
            string input = Console.ReadLine();
            this.controller.StartServer(input);
            Console.WriteLine("The server has been started!");
            Console.WriteLine("Press any key to stop the management server.");
            Console.ReadLine();
            StopServer();
        }

        private void SetVerbosity()
        {
            Console.WriteLine("The managment server is verbose now!");
            this.controller.SetVerbosity(true);
            printMenu();
        }

        private void StopServer()
        {
            Console.WriteLine("Stopping the management server!");
            this.controller.StopServer();
            Console.WriteLine("The management server has been stopped");
        }
    }
}