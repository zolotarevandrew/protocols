using System.Net;
using System.Net.Sockets;

namespace Client;

public class Worker : BackgroundService
{
    private readonly IPAddress _listeningAddress = IPAddress.Parse("127.0.0.1");
    private readonly int _listeningPort = 5555;
    private Memory<byte> _buffer = new byte[1024];
    private Socket _socket;

    public Worker()
    {
        _socket = new Socket(
            AddressFamily.InterNetwork,
            SocketType.Dgram,
            ProtocolType.Udp
        );
        _socket.Bind(new IPEndPoint(_listeningAddress, _listeningPort));
    }
    
    public override async Task StopAsync(CancellationToken token)
    {
        await _socket.DisconnectAsync(false, token);
        _socket.Dispose();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var recvResult = await _socket.ReceiveFromAsync(
                _buffer, 
                SocketFlags.None, 
                new IPEndPoint(_listeningAddress, _listeningPort), 
                stoppingToken);
            
            var recvPacket = _buffer.Slice(0, recvResult.ReceivedBytes);
            Console.WriteLine("Client received bytes length: " + recvPacket.Length);
            
            await Task.Delay(10000, stoppingToken);
        }
    }
}