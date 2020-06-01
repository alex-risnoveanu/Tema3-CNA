using Generated;
using Grpc.Core;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Server
{
    class LogInService : Generated.LogInService.LogInServiceBase
    {
        public override Task<Close> logIn(LogInRequest request, ServerCallContext context)
        {
            Console.WriteLine("\n--> " + request.Name + " has logged in! <--");;

            return Task.FromResult(new Close());
        }
        public override Task<Close> logOut(LogInRequest request, ServerCallContext context)
        {
            Console.WriteLine("\n--> " + request.Name + " has logged out! <--");

            return Task.FromResult(new Close());
        }

    }
}
