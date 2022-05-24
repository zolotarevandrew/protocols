using System.Collections.Generic;
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

        public override async Task<HelloReply> SayHello(IAsyncStreamReader<HelloRequest> requestStream,
            ServerCallContext context)
        {
            List<string> res = new List<string>();
            await foreach (var response in requestStream.ReadAllAsync())
            {
                res.Add(response.Name);
            }

            return new HelloReply
            {
                Message = "Hello " + string.Join(",", res)
            };
        }
    }
}