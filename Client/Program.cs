using Generated;
using Grpc.Core;
using Grpc.Core.Utils;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            const string Host = "localhost";
            const int Port = 16842;

            var channel = new Channel($"{Host}:{Port}", ChannelCredentials.Insecure);
            var client = new LogInRequest();

            Console.WriteLine("------------------------------------");
            Console.WriteLine("----------- Hello there ! ----------");
            Console.WriteLine("------------------------------------");

            do
            {
                Console.WriteLine("\nPlease enter your name: ");
                client.Name = Console.ReadLine();
                Console.WriteLine("\n------------------------------------");

            } while (isValid(client.Name) == false);

            var logIn = new ChatService.ChatServiceClient(channel);
            logIn.logIn(client);

            Console.WriteLine("Welcome " + client.Name + "!");
            Console.WriteLine("------------------------------------");

            var clientMessage = new ChatRequest();
            clientMessage.Name = client.Name;

            do
            {
                do
                {
                    Console.WriteLine("\nPlease enter your message: ");
                    clientMessage.Textmessage = Console.ReadLine();
                    Console.WriteLine("------------------------------------");

                } while (isValid(clientMessage.Textmessage) == false);

                var chat = new ChatService.ChatServiceClient(channel);
                chat.sendMessage(clientMessage);

                var stream = chat.chatStream(clientMessage);
                while (await stream.ResponseStream.MoveNext())
                {
                    var serverMessage = stream.ResponseStream.Current;

                    Console.WriteLine(serverMessage.Name + " says: " + serverMessage.Textmessage);
                    Console.WriteLine("------------------------------------");
                }


            } while (true);

            // Shutdown
            channel.ShutdownAsync().Wait();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        public static bool isValid(string currentNane)
        {
            if (currentNane == "")
            {
                Console.WriteLine("Ups! You forgot to fill in the field!");
                Console.WriteLine("------------------------------------");
                return false;
            }
            return true;
        }
    }
}
