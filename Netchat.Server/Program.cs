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


while (true) // Outermost loop to accept incoming client connections and handle their messages. It will continue to listen for new clients indefinitely.
{
    TcpClient client = await listener.AcceptTcpClientAsync();

    Console.WriteLine("Client connected!");

    NetworkStream stream = client.GetStream(); // Creates a stream object to send and receive data between client and server. Stream = Pipe between client and server for sending and receiving data.


    byte[] buffer = new byte[1024];

    byte[] usernameBuffer = new byte[1024];

    int bytesUsername = await stream.ReadAsync(usernameBuffer); 

    string username = Encoding.UTF8.GetString(usernameBuffer, 0, bytesUsername);

    while (true) // Inner loop to read messages from the connected client. It will continue to read messages until the client disconnects or sends an empty message.
    {

        int bytesRead = await stream.ReadAsync(buffer); //VERY IMPORTANT SERVER CODE LINE
        string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);

         if (bytesRead == 0)
         {
            Console.WriteLine($"Client {username} disconnected.");
            break; // Ignores whitespace and when bytesread == 0 upon client exit, breaks inner loop and disposes of client and stream objects, then returns to outer loop to await new client connections.
         }
  
        Console.WriteLine($"Received message from {username}: {message}");
    }

    client.Dispose();
    stream.Dispose();
    Console.WriteLine("Awaiting new client connections...");
}