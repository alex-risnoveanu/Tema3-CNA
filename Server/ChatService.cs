using Generated;
using Grpc.Core;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;

namespace Server
{
    internal class ChatService : Generated.ChatService.ChatServiceBase
    {
        //private static List<ChatRequest> users = new List<ChatRequest>();

        private static List<IServerStreamWriter<ChatResponse>> responseStreams = new List<IServerStreamWriter<ChatResponse>>();


        public override async Task chatStream(IAsyncStreamReader<ChatRequest> requestStream,
            IServerStreamWriter<ChatResponse> responseStream, ServerCallContext context)
        {
            responseStreams.Add(responseStream);

            while (await requestStream.MoveNext(CancellationToken.None))
            {
                var client = requestStream.Current;

                var message = new ChatResponse();

                message.Name = client.Name;
                message.Textmessage = client.Textmessage;

                foreach (var stream in responseStreams)
                {
                    await stream.WriteAsync(message);
                }
            }
        }

        public override Task<Close> sendMessage(ChatRequest request, ServerCallContext context)
        {
            Console.WriteLine("------------------------------------");
            Console.WriteLine(request.Name + " send a message!");
            Console.WriteLine("------------------------------------");

            //users.Add(request);
            return Task.FromResult(new Close());
        }


        //public override async Task chatStream(ChatRequest request, IServerStreamWriter<ChatRequest> responseStream, ServerCallContext context)
        //{
        //    foreach (var message in requestMessages)
        //    {
        //        await responseStream.WriteAsync(message);
        //    }
        //}
        //public override Task<Close> sendMessage(ChatRequest request, ServerCallContext context)
        //{
        //    Console.WriteLine("------------------------------------");
        //    Console.WriteLine(request.Name + " send a message!");
        //    Console.WriteLine("------------------------------------");

        //    requestMessages.Add(request);

        //    return Task.FromResult(new Close());
        //}


    }
}


