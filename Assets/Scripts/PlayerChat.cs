using Mirror;
using TMPro;
using UnityEngine;

public class PlayerChat : NetworkBehaviour
{

    [SerializeField] private TMP_InputField chatInput;
    [SerializeField] private TextMeshProUGUI chatDisplay;

    private void Start()
    {
        if (isLocalPlayer)
        {
            // UIManager에서 UI 요소를 가져옵니다.
            chatInput = UIManager.Instance.chatInputField;
            chatDisplay = UIManager.Instance.chatDisplayText;

        }
    }

    private void Update()
    {
        if (!isLocalPlayer || chatInput == null || chatDisplay == null)
            return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!string.IsNullOrEmpty(chatInput.text))
            {
                CmdSendMessage(chatInput.text);
                chatInput.text = string.Empty;
            }
        }
    }

    [Command]
    void CmdSendMessage(string message)
    {
        RpcReceiveMessage(message);
    }

    [ClientRpc]
    void RpcReceiveMessage(string message)
    {
        UIManager.Instance.AddMessage(message);
    }
}
