using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Client
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            var channel = GrpcChannel.ForAddress("http://localhost:7001");
            var client = new Greeter.GreeterClient(channel);
            while (!stoppingToken.IsCancellationRequested)
            {
                using var call = client.SayHello();
                for (var i = 0; i < 3; i++)
                {
                    await call.RequestStream.WriteAsync(new HelloRequest
                    {
                        Name = Guid.NewGuid().ToString()
                    }, stoppingToken);
                    await Task.Delay(1000, stoppingToken);
                    if (i == 2)
                    {
                        throw new InvalidOperationException("test");    
                    }
                }
                await call.RequestStream.CompleteAsync();
                var response = await call;
                Console.WriteLine($"Received: {response.Message}");
                Console.WriteLine();
            }
        }
    }
}