using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TMP_InputField chatInputField;
    public TextMeshProUGUI chatDisplayText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddMessage(string message)
    {
        if (chatDisplayText != null)
        {
            chatDisplayText.text += message + "\n";
        }
    }
}
