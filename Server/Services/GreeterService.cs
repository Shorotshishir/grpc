using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Server
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        // for unary response
        public override Task<HelloReply> SayHelloUnary(HelloRequest request, ServerCallContext context)
        {
            System.Console.WriteLine($"user: {request.Name}");
            return Task.FromResult(new HelloReply
            {
                Message = $"\nrespons to {request.Name}"
            });
        }

        // for server stream response
        public override async Task SayHelloServerStream(HelloRequest request, IServerStreamWriter<HelloReply> response, ServerCallContext context)
        {
            for (int i = 0; i < 10; i++)
            {
                await response.WriteAsync(new HelloReply
                {
                    Message = "Hello " + request.Name + i
                });
            }
        }

        // Client Streaming Response
        public override async Task<HelloReply> SayHello(IAsyncStreamReader<HelloRequest> request, ServerCallContext context)
        {
            while (await request.MoveNext())
            {
                System.Console.WriteLine(request.Current.Name);
            }
            return new HelloReply
            {
                Message = "received"
            };
        } 


        // // Bi - directional streaming
        // // Not Tested in unity yet
        /* public override async Task SayHelloBiDir(IAsyncStreamReader<HelloRequest> request,
                                            IServerStreamWriter<HelloReply> response,
                                            ServerCallContext context)
        {
             while (await request.MoveNext())
             {
                 
                 await response.WriteAsync(new HelloReply
                {
                    Message = "Hello " + request.Current.Name
                });
             }
        }  */
       
    }
}
