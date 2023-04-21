using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Unity.VisualScripting;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.InputSystem;
using ExitGames.Client.Photon;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField roomInputField;
    public GameObject lobbyPanel;
    public GameObject roomPanel;
    public TMP_Text roomName;

    public roomItem roomItemPrefab;
    List<roomItem> roomItemsList = new List<roomItem>();
    public Transform contentObject;

    public float timeBetweenUpdates = 1.5f;
    float nextUpdateTime;

    List<PlayerItem> playerItemsList = new List<PlayerItem>();
    public PlayerItem playerItemPrefab;
    public Transform playerItemParent;

    public GameObject playButton;
    public GameObject selectLevel;
    public levelInfoItem levelInfo;

    public bool allPlayersReady = false;

    public static PlayerControllerInputAsset input;
    private InputAction start;
    private InputAction leave;

    private void Awake()
    {
        input = new PlayerControllerInputAsset();
        start = input.UI.Start;
        leave = input.UI.Back;


        start.started += Start =>
        {
            if (playButton.activeInHierarchy)
                OnClickPlayButton();
        };

        leave.started += Leave =>
        {
            OnClickLeaveRoom();
        };

        start.Enable();
        leave.Enable();
        
   }


    private void Start()
    {
        if (PhotonNetwork.CurrentRoom != null)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.CurrentRoom.IsOpen = true;
                PhotonNetwork.CurrentRoom.IsVisible = true;
            }
            OnJoinedRoom();
        }
        else
        {
            PhotonNetwork.JoinLobby();
        }     


    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount >=1 && allPlayersReady)
        {
            playButton.SetActive(true);
        }
        else
        {
            playButton.SetActive(false);
        }

        //Turn on Level Changer if master Client
        if (PhotonNetwork.IsMasterClient)
        {
            selectLevel.SetActive(true);
        }
        else
        {
            selectLevel.SetActive(false);
        }
    }

    public void OnClickCreate()
    {
        if (roomInputField.text.Length >=1)
        {
            PhotonNetwork.CreateRoom(roomInputField.text, new RoomOptions() { MaxPlayers = 4, BroadcastPropsChangeToAll = true, DeleteNullProperties = true });
        }
    }

    public void Refresh()
    {
        PhotonNetwork.LoadLevel("New Lobby");
    }

    public override void OnJoinedRoom()
    {
        //Set ready to false by default on join
        Hashtable hash = new Hashtable();
        hash.Add("ready", false);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("left")
                && (bool)PhotonNetwork.LocalPlayer.CustomProperties["left"])
        {
            lobbyPanel.SetActive(true);
            roomPanel.SetActive(false);
            PhotonNetwork.LocalPlayer.CustomProperties["left"] = null;
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            lobbyPanel.SetActive(false);
            roomPanel.SetActive(true);
            UpdatePlayerList();
        }
        //roomName.text = "Room Name: " + PhotonNetwork.CurrentRoom.Name;
        roomName.text = PhotonNetwork.CurrentRoom.Name;
        
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (Time.time >= nextUpdateTime)
        {
            UpdateRoomList(roomList);
            nextUpdateTime = Time.time + timeBetweenUpdates;
        }
    }

    void UpdateRoomList(List<RoomInfo> list)
    {
        // Destroy old room items and clear list
        foreach (roomItem item in roomItemsList)
        {
            Destroy(item.gameObject);
        }
        roomItemsList.Clear();

        foreach (RoomInfo room in list)
        {
            if (room.IsOpen && room.IsVisible)
            {
                roomItem newRoom = Instantiate(roomItemPrefab, contentObject);
                newRoom.SetRoomName(room.Name);
                roomItemsList.Add(newRoom);
            }
            
        }
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        roomPanel.SetActive(false);
        lobbyPanel.SetActive(true);
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("ready"))
        {
            PhotonNetwork.LocalPlayer.CustomProperties["ready"] = null;
        }
        
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    void UpdatePlayerList()
    {
        // Destroy old player items and clear list
        
        foreach (PlayerItem item in playerItemsList)
        {
            Destroy(item.gameObject);
        }
        playerItemsList.Clear();

        if (PhotonNetwork.CurrentRoom == null)
        {
            return;
        }

        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            PlayerItem newPlayerItem = Instantiate(playerItemPrefab, playerItemParent);
            newPlayerItem.SetPlayerInfo(player.Value);

            if (player.Value == PhotonNetwork.LocalPlayer)
            {
                newPlayerItem.ApplyLocalChanges();
            }

            playerItemsList.Add(newPlayerItem);

            for(int i = 0; i < playerItemsList.Count - 1; i++)
            {
                if(playerItemsList[i].GetPlayer().ActorNumber > playerItemsList[i+1].GetPlayer().ActorNumber)
                {
                    PlayerItem temp = playerItemsList[i];
                    playerItemsList[i] = playerItemsList[i + 1];
                    playerItemsList[i + 1] = temp;
                }
                playerItemsList[i].transform.SetSiblingIndex(i);
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }

    public void OnClickPlayButton()
    {

        input.Dispose();
        PhotonNetwork.LoadLevel(levelInfo.getSceneName());      
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {

        if (changedProps.ContainsKey("left") && (bool)changedProps["left"]) 
        {
            PhotonNetwork.LoadLevel("New Lobby");
        }
        
        bool allReady = true;

        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            if (player.Value.CustomProperties.TryGetValue("ready", out object readyout) && !(bool) readyout)
            {
                allReady = false;
            }
        }
        allPlayersReady = allReady;
    }
}
