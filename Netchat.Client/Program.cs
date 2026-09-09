/**
 * IP/Local Host: 127.0.0.1
 * Port: 5000
 * 
 * Function: Connect to server on localhost and port.
 */
using System.Net.Sockets;

TcpClient client = new TcpClient();

Console.WriteLine("Connecting to Server...");

await client.ConnectAsync("127.0.0.1", 5000);

Console.WriteLine("Successfully connected to server on port 5000!");

Console.ReadLine();
