using System.Net;
using System.Net.Sockets;
using System.Text;

class Server
{
    private static TcpListener listener;
    private static readonly int port = 8888;
    private static List<TcpClient> clients = new List<TcpClient>();
    private static object lockObject = new object();

    static void Main(string[] args)
    {
        listener = new TcpListener(IPAddress.Any, port);
        listener.Start();
        Console.WriteLine("서버시작 포트 " + port);

        while (true)
        {
            TcpClient client = listener.AcceptTcpClient();
            lock (lockObject)
            {
                clients.Add(client);
            }
            Console.WriteLine("클라이언트 연결");
            Thread clientThread = new Thread(HandleClient);
            clientThread.Start(client);
        }
    }



	private static void HandleClient(object obj)
    {
        TcpClient client = (TcpClient)obj;
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];
        int bytesRead;


        try
        {
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine("메시지 : " + message); // 메시지를 콘솔에 출력
                BroadcastMessage(message, client);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"클라이언트 연결종료 원인 : {ex.Message}");
        }
        finally
        {
            lock (lockObject)
            {
                clients.Remove(client);
                client.Close();
            }
        }

    }
	
    private static void BroadcastMessage(string message, TcpClient excludeClient)
    {
        lock (lockObject)
        {
            byte[] response = Encoding.UTF8.GetBytes(message);
            foreach (TcpClient client in clients)
            {
                // 접속하지 못한 client가 아니라면(접속 성공한 클라이언트)
                if (client != excludeClient)
                {
                    NetworkStream stream = client.GetStream();
                    stream.Write(response, 0, response.Length);
                }

            }

        }
    }	
}