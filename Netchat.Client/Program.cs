/**
 * IP/Local Host: 127.0.0.1
 * Port: 5000
 * 
 * Function: Connect to server on localhost and port.
 */
using System.Net.Sockets;
using System.Text;



string? restartInput;

do //Outermost loop to check for user input and restart session if desired. If user enters "Y", it will restart the session, if user enters "N", it will exit the application.
{
        bool isConnected = false;
        bool isMsg = false;

        TcpClient client = new TcpClient();

    while (!isConnected) //Outer loop to check for server availability and connection status. If server is unavailable, it will retry every 5 seconds until a connection is established.
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

            NetworkStream stream = client.GetStream(); //Creates a stream object to send and receive data between client and server. Stream = Pipe between client and server for sending and receiving data.

            Console.WriteLine("Welcome to NetChat Client!\nPlease enter your username: ");
            string? userName = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(userName))
            {
                Console.WriteLine($"Hello {userName}! You can now send messages to the server.");

                byte[] username = Encoding.UTF8.GetBytes(userName);
                await stream.WriteAsync(username);
            }

                while (!isMsg) //Inner loop to check for user input and send messages to the server. If user enters "exit", it will break the loop and dispose of client and stream objects, then prompt user to restart session or exit.
                {
                    Console.Write($"{userName}: ");
                    string? userInput = Console.ReadLine();
                    isMsg = true;
                    if (string.IsNullOrWhiteSpace(userInput))
                    {
                        Console.WriteLine("No message entered.");
                    }

                    if (userInput.ToLower() == "exit")
                    {
                        Console.WriteLine("Exiting user session...");
                        break; //Breaks inner loop and disposes of client and stream objects, then prompts user to restart session or exit.
                    }

                    byte[] data = Encoding.UTF8.GetBytes(userInput); //Converts user input to byte array for sending to server.
                    await stream.WriteAsync(data); //Sends user input to server as byte array. Stream = Pipe between client and server for sending and receiving data.
                    isMsg = false;
                }

            stream.Dispose(); //Client-side object cleanup once exit break statement is reached
            client.Dispose(); //Client-side object cleanup once exit break statement is reached

            Console.Write("Restore Client Session? Y/N: ");
            restartInput = Console.ReadLine();

} while (restartInput?.ToUpper() == "Y"); //Loop condition to check for user input and restart session if desired. If user enters "Y", it will restart the session, if user enters "N", it will exit the application.


if (restartInput?.ToUpper() == "N")
{
    Console.WriteLine("Exiting NetChat Client...");
    return;
}

Console.ReadLine();


