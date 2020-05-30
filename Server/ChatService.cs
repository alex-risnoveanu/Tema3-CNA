using Generated;
using Grpc.Core;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server
{
    internal class ChatService : Generated.ChatService.ChatServiceBase , ISubject
    {
        private static List<ChatRequest> requestMessages = new List<ChatRequest>();


        public List<IObserver> observers = new List<IObserver>();
        public List<LogInRequest> observerslog = new List<LogInRequest>();


        public override Task<Close> logIn(LogInRequest request, ServerCallContext context)
        {
            Console.WriteLine("\n--> " + request.Name + " has logged in! <--");
            observerslog.Add(request);

            return Task.FromResult(new Close());
        }

        public override async Task chatStream(ChatRequest request, IServerStreamWriter<ChatRequest> responseStream, ServerCallContext context)
        {
            foreach (var message in requestMessages)
            {
                await responseStream.WriteAsync(message);
            }
        }

        public void Notify()
        {
            foreach(IObserver observer in observers)
            {
                observer.Update();
            }
        }

        public override Task<Close> sendMessage(ChatRequest request, ServerCallContext context)
        {
            Console.WriteLine("------------------------------------");
            Console.WriteLine(request.Name + " send a message!");
            Console.WriteLine("------------------------------------");

            requestMessages.Add(request);
            this.Notify();

            return Task.FromResult(new Close());
        }

        public void Subscribe(IObserver observer)
        {
            observers.Add(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            observers.Remove(observer);
        }
    }
}


