/**
 * IP/Local Host: 127.0.0.1
 * Port: 5000
 * 
 * Function: Connect to server on localhost and port.
 */
using System.Net.Sockets;
using System.Text;

bool isConnected = false;

TcpClient client = new TcpClient();

while (!isConnected)
{
    try
    {
        Console.WriteLine("Connecting to Server...");
        await client.ConnectAsync("127.0.0.1", 5000);
        isConnected = true;
        Console.WriteLine("Successfully connected to server on port 5000!");
    }
    catch (SocketException ex)
    {
        Console.WriteLine("Server Unavailable");
        Console.WriteLine("Retrying in 5 seconds...");
        await Task.Delay(5000);
        client = new TcpClient();
        }
    }

NetworkStream stream = client.GetStream();

string message = "Hello from client!";

byte[] data = Encoding.UTF8.GetBytes(message);

await stream.WriteAsync(data);

Console.WriteLine($"Sent message to server: {message}");

Console.ReadLine();
