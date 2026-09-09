/**
 * Create TCP listener that listens for incoming connections on port 5000.
 */
using System.Net;
using System.Net.Sockets;
using System.Text;

TcpListener listener = new TcpListener(IPAddress.Any, 5000);

listener.Start();

Console.WriteLine("NetChat Server started.");
Console.WriteLine("Listening on port 5000...");
Console.WriteLine("Waiting for clients to connect...");


while (true)
{
    TcpClient client = await listener.AcceptTcpClientAsync();

    Console.WriteLine("Client connected!");

    NetworkStream stream = client.GetStream();


    byte[] buffer = new byte[1024];

    byte[] usernameBuffer = new byte[1024];

    int bytesUsername = await stream.ReadAsync(usernameBuffer);

    string username = Encoding.UTF8.GetString(usernameBuffer, 0, bytesUsername);

    while (true)
    {

        int bytesRead = await stream.ReadAsync(buffer);

        if (bytesRead == 0)
        {
            Console.WriteLine($"Client {username} disconnected.");
            break;
        }

        string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);

        Console.WriteLine($"Received message from {username}: {message}");

    }

    client.Dispose();
    stream.Dispose();
    Console.WriteLine("Awaiting new client connections...");
}