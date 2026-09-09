/**
 * IP/Local Host: 127.0.0.1
 * Port: 5000
 * 
 * Function: Connect to server on localhost and port.
 */
using System.Net.Sockets;
using System.Text;
string? restartInput;

do
{
        bool isConnected = false;
        bool isMsg = false;

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

        Console.WriteLine("Welcome to NetChat Client!\nPlease enter your name: ");
        string? userName = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(userName))
            {
                Console.WriteLine($"Hello {userName}! You can now send messages to the server.");

                byte[] username = Encoding.UTF8.GetBytes(userName);
                await stream.WriteAsync(username);
            }


            while (!isMsg)
            {
                Console.Write($"{userName}: ");
                string? userInput = Console.ReadLine();
                isMsg = true;
                if (string.IsNullOrWhiteSpace(userInput))
                {
                    Console.WriteLine("No message entered.");
                    continue;
                }

                if (userInput.ToLower() == "exit")
                {
                    Console.WriteLine("Exiting session...");
                    break;
                }
                byte[] data = Encoding.UTF8.GetBytes(userInput);
                await stream.WriteAsync(data);
                //Console.WriteLine("Sent message to server!");
                isMsg = false;
            }

        stream.Dispose();
        client.Dispose();
        Console.Write("Restore Client Session? Y/N: ");
            restartInput = Console.ReadLine();

} while (restartInput?.ToUpper() == "Y");


if (restartInput?.ToUpper() == "N")
{
    Console.WriteLine("Exiting NetChat Client...");
    return;
    
}

Console.ReadLine();


