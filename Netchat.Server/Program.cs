using System.Net;
using System.Net.Sockets;


TcpListener listener = new TcpListener(IPAddress.Any, 5000);

listener.Start();

Console.WriteLine("NetChat Server started.");
Console.WriteLine("Listening on port 5000...");
Console.WriteLine("Waiting for clients to connect...");

TcpClient client = await listener.AcceptTcpClientAsync();

Console.WriteLine("Client connected!");

Console.ReadLine();