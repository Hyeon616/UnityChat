using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Client : MonoBehaviour
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

        inputField.ActivateInputField();
        inputField.Select();
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
            Debug.Log($"������ ���� ���� �޽��� : {message}");
        }


        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!string.IsNullOrWhiteSpace(inputField.text))
            {
                SendMessageToServer();
                Debug.Log("�Է�");
            }
        }

    }


    private void ConnectToServer()
    {
        try
        {
            client = new TcpClient(serverAddress, port);
            stream = client.GetStream();
            Debug.Log("���� ����");
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
            if (!string.IsNullOrEmpty(message))
            {
                inputField.text = string.Empty;
                receivevdMeeageText.text += "\n" + message;

                byte[] data = Encoding.UTF8.GetBytes(message);
                stream.Write(data, 0, data.Length);
                Debug.Log($"������ ���� : {message}");

                inputField.text = string.Empty;
                inputField.ActivateInputField();
            }

        }
        else
        {
            Debug.Log("�޽����� �������� ���߽��ϴ�.");
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
