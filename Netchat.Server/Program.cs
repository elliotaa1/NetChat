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

TcpClient client = await listener.AcceptTcpClientAsync();

Console.WriteLine("Client connected!");

NetworkStream stream = client.GetStream();

byte[] buffer = new byte[1024];

int bytesRead = await stream.ReadAsync(buffer);

string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);

Console.WriteLine($"Received message from client: {message}");

string reply = "Hello Client! Message received.";

byte[] replyData = Encoding.UTF8.GetBytes(reply);

await stream.WriteAsync(replyData);

Console.WriteLine("Reply sent to client...");

Console.ReadLine();