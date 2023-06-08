using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;
using UnityEngine.InputSystem;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    public TMP_InputField usernameInput;
    public TMP_Text buttonText;
    private PlayerControllerInputAsset input;
    public InputAction enter { get; private set; }
    [SerializeField] private UIAudio uiAudio;

    public void OnClickConnect()
    {
        // If the nickname is at least 1 character long
        if (usernameInput.text.Length >= 1)
        {
            //Set the player nickname
            PhotonNetwork.NickName = usernameInput.text;
            buttonText.text = "Connecting...";
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        SceneManager.LoadScene("FINAL LOBBY SELECTION");
    }

    private void Awake()
    {
        input = new PlayerControllerInputAsset();
        enter = input.UI.Enter;

        enter.performed += EnterButton =>
        {
            Debug.Log("hit enter");
            OnClickConnect();
        };
        enter.Enable();
    }
}
