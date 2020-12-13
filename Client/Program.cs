using System;
using System.Net.Http;
using Grpc.Net.Client;
using System.Threading.Tasks;
using Grpc.Core;

namespace Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            using var channel =  GrpcChannel.ForAddress("http://192.168.1.228:5001"); // use your own open IP address and port
            var client  = new Greeter.GreeterClient(channel);

            // for Uniary call
            /*
            var reply = await client.SayHelloAsync(
                new HelloRequest{
                    Name = "Client"
                }
            );
            */

            // server streaming
            
            using var call=  client.SayHello (
                new HelloRequest {Name= "Shorotshishir"}
            );
            while (await call.ResponseStream.MoveNext())
            {
                Console.WriteLine($"Greetings {call.ResponseStream.Current.Message}"); 
                
            }
            

            // client streaming
            /* using var call = client.SayHello();
            for (int i = 0; i < 5; i++)
            {
                await call.RequestStream.WriteAsync(
                    new HelloRequest {
                        Name = "Shorotshishir"
                    }
                );
            }
            await call.RequestStream.CompleteAsync();
            var response = await call;
            System.Console.WriteLine($" Did you receive ? {response.Message}");
            */

            // Bi directional 
            /* 
            using var call = client.SayHello();
            var readTask = Task.Run(async ()=> {
                while(await call.ResponseStream.MoveNext()){
                    System.Console.WriteLine($"Server Stream: {call.ResponseStream.Current.Message}");
                }
            });

            for (int i = 0; i < 5; i++)
            {
                await call.RequestStream.WriteAsync(
                    new HelloRequest {
                        Name = "Shorotshishir"
                    }
                );
            } 
            */
            
            Console.WriteLine("Press any key to exit...");
            // Console.ReadKey();
        }
    }
}
