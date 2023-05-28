using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

using UnityEngine;

using UnityEngine.InputSystem;
using TMPro;


public class CharacterSelect : MonoBehaviourPunCallbacks
{
    public TMP_Text playerName;
    //Local Player properties
    public ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();

    //Image of the player avatar
    public Image playerAvatar;

    //Sprites of the player avatar for the image
    public Sprite[] avatars;
    public GameObject[] splashes;
    public Sprite[] signs; 

    //Variables for controlling input
    private static PlayerControllerInputAsset input;
    private InputAction select;
    private InputAction move;

    //Local Player 
    private Player player = PhotonNetwork.LocalPlayer;
    
    //Ready Up Text
    public TMP_Text readyUpText;

    //Ready Up Bool
    private bool isReady = false;

    //Audio
    characterSelectAudio CharacterSelectAudio;

    //Descriptions
    public CharacterDescription characterDescription;

    void Start()
    {
        CharacterSelectAudio = GetComponent<characterSelectAudio>();
        readyUpText.text = "Lock In?";


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

        playerName.text =player.NickName;

        characterDescription.ChangeCharacter(0);
        select.Enable();
        move.Enable();
    }



    public void OnClickLeft()
    {
        if (isReady)
            return;

        if ((int)playerProperties["playerAvatar"] == 0)
        {
            playerProperties["playerAvatar"] = avatars.Length - 1;
        }
        else
        {
            playerProperties["playerAvatar"] = (int)playerProperties["playerAvatar"] - 1;
        }
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
        characterDescription.ChangeCharacter((int)playerProperties["playerAvatar"]);
    }

    public void OnClickRight()
    {
        if (isReady)
            return;

        if ((int)playerProperties["playerAvatar"] == avatars.Length - 1)
        {
            playerProperties["playerAvatar"] = 0;
        }
        else
        {
            playerProperties["playerAvatar"] = (int)playerProperties["playerAvatar"] + 1;
        }
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
        characterDescription.ChangeCharacter((int)playerProperties["playerAvatar"]);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (player == targetPlayer)
        {
            UpdatePlayerItem(targetPlayer);
        }

        if (player.CustomProperties.TryGetValue("ready", out object readyout))
        {
            isReady = (bool)readyout;
        }
    }

    void UpdatePlayerItem(Player _player)
    {
        if (_player.CustomProperties.ContainsKey("playerAvatar"))
        {
            //playerAvatar.sprite = avatars[(int)player.CustomProperties["playerAvatar"]];
            toggleOnSplash((int)player.CustomProperties["playerAvatar"]);
            playerProperties["playerAvatar"] = (int)player.CustomProperties["playerAvatar"];
            characterDescription.ChangeCharacter((int)playerProperties["playerAvatar"]);
        }
        else
        {
            playerProperties["playerAvatar"] = 0;
            if (PhotonNetwork.LocalPlayer == _player)
                PhotonNetwork.SetPlayerCustomProperties(playerProperties);
        }
    }

    void toggleOnSplash(int i)
    {
        foreach (GameObject go in splashes)
        {
            go.SetActive(false);
        }
        splashes[i].SetActive(true);
    }

    public void ReadyUpToggle()
    {
        isReady = !isReady;

        if (isReady)
        {
            readyUpText.text = "Unselect?";
            move.Disable();
        }
        else
        {
            move.Enable();
            readyUpText.text = "Lock In?";
        }

        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash.Add("ready", isReady);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

        if (isReady)
        {
            CharacterSelectAudio.CallCharacterLock();
        }
    }
}
