using System;
using System.Collections.Generic;
using System.Linq;

namespace Server
{
    class Server : IDisposable
    {
        public Grpc.Core.Server GrpcServer { get; private set; }

        public Action CloseServerAction { get; set; }

        public IEnumerable<Grpc.Core.ServerServiceDefinition> Services
        {
            get
            {
                yield return Generated.ChatService.BindService(new ChatService());
                yield return Generated.LogInService.BindService(new LogInService());
            }
        }

        public Server(string host, int port)
        {
            GrpcServer = new Grpc.Core.Server()
            {
                Ports = { new Grpc.Core.ServerPort(host, port, Grpc.Core.ServerCredentials.Insecure) }
            };

            LoadServices();
        }

        public void Start()
        {
            GrpcServer.Start();

            Console.WriteLine(string.Format("Server started ({0}:{1}).", Configuration.HOST, Configuration.PORT));
        }

        private void LoadServices()
        {
            Services.ToList().ForEach(service => GrpcServer.Services.Add(service));
        }

        public void Dispose()
        {
            CloseServerAction.Invoke();
            GrpcServer.ShutdownAsync().Wait();
            var port = GrpcServer.Ports.FirstOrDefault();
            Console.WriteLine("Server closed ({0}:{1}).", Configuration.HOST, Configuration.PORT);
        }
    }
}
