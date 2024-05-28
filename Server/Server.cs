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

}