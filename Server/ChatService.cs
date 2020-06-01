using Generated;
using Grpc.Core;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Server
{
    internal class ChatService : Generated.ChatService.ChatServiceBase
    {
        private static List<ChatRequest> requestMessages = new List<ChatRequest>();
        private static List<LogInRequest> observers = new List<LogInRequest>();

        public override Task<Close> logIn(LogInRequest request, ServerCallContext context)
        {
            Console.WriteLine("\n--> " + request.Name + " has logged in! <--");

            observers.Add(request);
            return Task.FromResult(new Close());
        }
        public override Task<Close> logOut(LogInRequest request, ServerCallContext context)
        {
            Console.WriteLine("\n--> " + request.Name + " has logged out! <--");

            //observers.Add(request);
            return Task.FromResult(new Close());
        }
    

        public override async Task chatStream(ChatRequest request, IServerStreamWriter<ChatRequest> responseStream, ServerCallContext context)
        {
            foreach (var message in requestMessages)
            {
                await responseStream.WriteAsync(message);
            }
        }
        public override Task<Close> sendMessage(ChatRequest request, ServerCallContext context)
        {
            Console.WriteLine("------------------------------------");
            Console.WriteLine(request.Name + " send a message!");
            Console.WriteLine("------------------------------------");

            requestMessages.Clear();
            requestMessages.Add(request);
            //this.Notify();

            return Task.FromResult(new Close());
        }
    }
}


