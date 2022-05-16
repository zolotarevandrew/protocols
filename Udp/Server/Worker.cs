using System.Net;
using System.Net.Sockets;

namespace Server;

public class Worker : BackgroundService
{
    private readonly IPAddress _address = IPAddress.Parse("127.0.0.1");
    private readonly int _sendPort = 5555;
    private Memory<byte> _buffer = new byte[1024];
    private Socket _socket;

    public Worker()
    {
        _socket = new Socket(
            AddressFamily.InterNetwork,
            SocketType.Dgram,
            ProtocolType.Udp
        );
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
            var bytes = new byte[_buffer.Length];
            for (int i = 0; i < 1024; i++)
            {
                bytes[i] = 1;
            }
            bytes.CopyTo(_buffer);
            var recvResult = await _socket.SendToAsync(
                _buffer, 
                SocketFlags.None, 
                new IPEndPoint(_address, _sendPort), 
                stoppingToken);
            
            Console.WriteLine("Server sended bytes," + recvResult);
            await Task.Delay(10000, stoppingToken);
        }
    }
}