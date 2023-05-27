using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class PlayerItem : MonoBehaviourPunCallbacks
{
    public TMP_Text playerName;

    public GameObject playerItemButton;

    public TMP_Text readyUpText;

    public ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
    public Image playerAvatar;
    public Sprite[] avatars;

    Player player;
    characterSelectAudio characterSelect;

    private static PlayerControllerInputAsset input;
    private InputAction select;
    private InputAction move;

    bool isReady = false;

    public void Awake()
    {
        characterSelect = GetComponent<characterSelectAudio>();
        readyUpText.text = "Ready Up";


        input = LobbyManager.input;
        select = input.UI.Select;
        move = input.UI.UIMove;


        select.started += Select =>
        {
            ReadyUpToggle();
        };

        move.started += Move =>
        {
            Vector2 movement = move.ReadValue<Vector2>();
            if (movement.x < 0)
                OnClickLeft();
            else
                OnClickRight();
        };


        select.Enable();
        move.Enable();

    }

    public Player GetPlayer()
    {
        return player;
    }

    public void ApplyLocalChanges()
    {


        playerItemButton.SetActive(true);

    }

    public void SetPlayerInfo(Player _player)
    {
        playerName.text = _player.NickName;
        player = _player;
        if (player.CustomProperties.TryGetValue("ready", out object readyout))
        {
            if ((bool)readyout)
            {
                isReady = (bool) readyout; 
                readyUpText.text = "Unselect";
                Debug.Log("Existing player, unselect time");
            }
        }
    }

    public void OnClickLeft()
    {
        if ((int)playerProperties["playerAvatar"] == 0)
        {
            playerProperties["playerAvatar"] = avatars.Length-1;
        }
        else
        {
            playerProperties["playerAvatar"] = (int)playerProperties["playerAvatar"] - 1;
        }
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }

    public void OnClickRight()
    {
        if ((int)playerProperties["playerAvatar"] == avatars.Length-1)
        {
            playerProperties["playerAvatar"] = 0;
        }
        else
        {
            playerProperties["playerAvatar"] = (int)playerProperties["playerAvatar"] + 1;
        }
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (player == targetPlayer)
        {
            UpdatePlayerItem(targetPlayer);
        }

        if (player.CustomProperties.TryGetValue("ready", out object readyout))
        {
            
        }
    }

    void UpdatePlayerItem(Player _player)
    {
        if (_player.CustomProperties.ContainsKey("playerAvatar"))
        {
            playerAvatar.sprite = avatars[(int)player.CustomProperties["playerAvatar"]];
            playerProperties["playerAvatar"] = (int)player.CustomProperties["playerAvatar"];
        }
        else
        {
            playerProperties["playerAvatar"] = 0;
            if (PhotonNetwork.LocalPlayer == _player)
                PhotonNetwork.SetPlayerCustomProperties(playerProperties);
        }
    }

    public void ReadyUpToggle()
    {
        isReady = !isReady;

        if (isReady)
        {
            readyUpText.text = "Unselect";
            move.Disable();
        }
        else
        {
            move.Enable();
            readyUpText.text = "Ready Up";
        }

        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash.Add("ready", isReady);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        

        if (isReady)
        {
            characterSelect.CallCharacterLock();
        }

    }
}
