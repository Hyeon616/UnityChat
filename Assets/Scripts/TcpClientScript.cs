using System;
using System.Net.Sockets;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class TcpClientScript : MonoBehaviour
{
    public TMP_InputField inputField;
    public Button sendButton;
    public TextMeshProUGUI receivevdMeeageText;

    private TcpClient client;
    private NetworkStream stream;
    private byte[] buffer = new byte[1024];
    public string serverAddress = "127.0.0.1";
    public int port = 8888;

    void Start()
    {
        ConnectToServer();
        
    }

    private void OnEnable()
    {
        sendButton.onClick.AddListener(SendMessageToServer);
    }

    private void OnDisable()
    {
        sendButton.onClick.RemoveListener(SendMessageToServer);
    }

    void Update()
    {
        if (stream != null && stream.DataAvailable)
        {
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            receivevdMeeageText.text += "\n" + message;
            Debug.Log($"서버로 부터 받은 메시지 : {message}");
        }

        if(Input.GetKeyUp(KeyCode.KeypadEnter))
        {
            SendMessageToServer();
        }
    }
 

    private void ConnectToServer()
    {
        try
        {
            client = new TcpClient(serverAddress, port);
            stream = client.GetStream();
            Debug.Log("서버 접속");
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    void SendMessageToServer()
    {
        if (stream != null)
        {
            string message = inputField.text;
            byte[] data = Encoding.UTF8.GetBytes(message);
            stream.Write(data, 0, data.Length);
            Debug.Log($"서버로 전송 : {message}");

        }
    }

    private void OnApplicationQuit()
    {
        if (stream != null)
        {
            stream.Close();
        }
        if (client != null)
        {
            client.Close();
        }
    }
}
