/**
 * Create TCP listener that listens for incoming connections on port 5000.
 */
using System.Net;
using System.Net.Sockets;
using System.Text;

List<TcpClient> connectedClients = new List<TcpClient>();

TcpListener listener = new TcpListener(IPAddress.Any, 5000);

listener.Start();

Console.WriteLine("NetChat Server started.");
Console.WriteLine("Listening on port 5000...");
Console.WriteLine("Waiting for clients to connect...");


while (true) // Outermost loop to accept incoming client connections and handle their messages. It will continue to listen for new clients indefinitely.
{
    TcpClient client = await listener.AcceptTcpClientAsync();
    connectedClients.Add(client);
    Console.WriteLine("Client connected!");

    _ = HandleClientAsync(client, connectedClients); 
}


static async Task BroadcastMessageAsync(string message, TcpClient sender, List<TcpClient> connectedClients)
{
    byte[] data = Encoding.UTF8.GetBytes(message);

    foreach(TcpClient client in connectedClients)
    {
        if(client != sender)
        {
            NetworkStream stream = client.GetStream();
            await stream.WriteAsync(data);
        }
    }
}


static async Task HandleClientAsync(TcpClient client, List<TcpClient> connectedClients)
{
    NetworkStream stream = client.GetStream(); // Creates a stream object to send and receive data between client and server. Stream = Pipe between client and server for sending and receiving data.

    byte[] buffer = new byte[1024];

    byte[] usernameBuffer = new byte[1024];

    int bytesUsername = await stream.ReadAsync(usernameBuffer);

    string usernamePacket = Encoding.UTF8.GetString(usernameBuffer, 0, bytesUsername);
    string username = usernamePacket.Substring("USERNAME|".Length);
    Console.WriteLine($"{username} joined the server.");

    while (true) // Inner loop to read messages from the connected client. It will continue to read messages until the client disconnects or sends an empty message.
    {

        int bytesRead = await stream.ReadAsync(buffer); //VERY IMPORTANT SERVER CODE LINE

        if (bytesRead == 0)
        {
            Console.WriteLine($"Client {username} disconnected.");
            break; // Ignores whitespace and when bytesread == 0 upon client exit, breaks inner loop and disposes of client and stream objects, then returns to outer loop to await new client connections.
        }

        string messagePacket = Encoding.UTF8.GetString(buffer, 0, bytesRead);


        if (messagePacket.StartsWith("MESSAGE|"))
        {
            string message = messagePacket.Substring("MESSAGE|".Length);

            Console.WriteLine($"Received message from {username}: {message}");
            await BroadcastMessageAsync($"{username}: {message}", client, connectedClients);
        }

    }

    connectedClients.Remove(client);
    client.Dispose();
    stream.Dispose();
    Console.WriteLine("Awaiting new client connections...");
}

    