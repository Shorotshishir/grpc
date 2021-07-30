using System;
using System.Net.Http;
using Grpc.Net.Client;
using System.Threading.Tasks;
using Grpc.Core;
using System.Net;
using System.Net.Sockets;

namespace Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            using var channel =  GrpcChannel.ForAddress($"http://{GetLocalIp()}:5001");
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
            
            using var call=  client.SayHelloServerStream (
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

        private static object GetLocalIp()
        {
            string localIP;
            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                localIP = endPoint.Address.ToString();
            }
            return localIP;
        }
    }
}
