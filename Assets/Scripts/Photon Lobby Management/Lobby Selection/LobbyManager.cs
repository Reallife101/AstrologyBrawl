using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.InputSystem;
using ExitGames.Client.Photon;
using System;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    //Room Creation
    public TMP_InputField roomInputField;
    public TMP_Text roomName;
    public roomItem roomItemPrefab;
    List<roomItem> roomItemsList = new List<roomItem>(); //List of rooms created.
    public Transform contentObject; //Transform for where to put the rooms in the screen

    //GameObjects of the lobby and room
    public GameObject lobbyPanel;
    public GameObject roomPanel;
    
    //Time for updates between refreses for the room list
    public float timeBetweenUpdates = 1.5f;
    float nextUpdateTime;

    //The GameObjects of the other players' characters
    [SerializeField]
    private List<GameObject> otherPlayersViewsGameObject = new List<GameObject>();

    //GameObjects associated with the players and their index in the otherPlayersGameObject 
    private Dictionary<int, KeyValuePair<int, GameObject>> otherPlayersViews = new Dictionary<int, KeyValuePair<int, GameObject>>();

    public CharacterSelect character; 

    //Play button stuff
    public GameObject playButton;
    public bool allPlayersReady = false;

    //Variables for Selecting the Level and getting the info from the level selected
    public GameObject selectLevel;
    public levelInfoItem levelInfo;

    //Variables for the InputSystem and binding methods to button calls
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
        //Activates PlayButton there is enough players and all players in the room are ready
        if (PhotonNetwork.IsMasterClient && 
                PhotonNetwork.CurrentRoom.PlayerCount >=1 && allPlayersReady)   
            playButton.SetActive(true);
        else
            playButton.SetActive(false);
        

        //Turn on Level Changer if master Client
        if (PhotonNetwork.IsMasterClient)
            selectLevel.SetActive(true);
        else
            selectLevel.SetActive(false);

    }


    void UpdateRoomList(List<RoomInfo> list)
    {
        // Destroy old room items and clear list
        foreach (roomItem item in roomItemsList)
        {
            Destroy(item.gameObject);
        }
        roomItemsList.Clear();

        //Creates new room items to show the room that was recently removed or created
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


    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {

        if (changedProps.ContainsKey("left") && (bool)changedProps["left"])
        {
            PhotonNetwork.LoadLevel("New Lobby");
        }


        if (!targetPlayer.IsLocal && changedProps.ContainsKey("playerAvatar"))
        {
            UpdatePlayerList(targetPlayer);
        }

        //CHANGE THISSSSSSSSSSSSSS

        bool allReady = true;

        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            if (player.Value.CustomProperties.TryGetValue("ready", out object readyout) && !(bool)readyout)
            {
                allReady = false;
            }
        }
        allPlayersReady = allReady;
    }


    void UpdatePlayerList(Player other)
    {
        int hashcode = other.GetHashCode();
        Transform transform = otherPlayersViews[hashcode].Value.transform;
        
        Image avatarImg = transform.GetChild(0).GetComponent<Image>();
        TMP_Text playerName = transform.GetChild(1).GetComponent<TMP_Text>();

        avatarImg.sprite = character.avatars[0];

        if (other.CustomProperties.ContainsKey("playerAvatar"))
            avatarImg.sprite = character.signs[(int) other.CustomProperties["playerAvatar"]];

        playerName.text = other.NickName; 
    }

    private void AddPlayerToDictionary(Player other)
    {
        int hashcode = other.GetHashCode();

        if (otherPlayersViews.ContainsKey(hashcode))
            return;

        int index = 0;

        for (; index < otherPlayersViewsGameObject.Count; ++index)
            if (!otherPlayersViewsGameObject[index].activeInHierarchy)
            {
                otherPlayersViewsGameObject[index].SetActive(true);
                break;
            }

        otherPlayersViews.Add(hashcode, new KeyValuePair<int, GameObject>(index, otherPlayersViewsGameObject[index]));
    }

    private void DeletePlayerFromDictionary(Player other)
    {
        int hashcode = other.GetHashCode();

        if (!otherPlayersViews.ContainsKey(hashcode))
            return;

        otherPlayersViews[hashcode].Value.SetActive(false);

        otherPlayersViews.Remove(hashcode);
    }

    //////// Button Callbacks Methods ///////

    public void OnClickPlayButton()
    {
        //Closes the room to not allow new players to join and disables input. Lastly, loads the Level chosen by the player. 
        input.Dispose();
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        PhotonNetwork.LoadLevel(levelInfo.getSceneName());
        //PhotonNetwork.LoadLevel("Battlefield");
    }
    
    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void Refresh()
    {
        // Refreshes the rooms by reloading the scene //
        PhotonNetwork.LoadLevel("New Lobby");
    }

    public void OnClickCreate()
    {
        // Creates a new room when there is text in roomInputField
        if (roomInputField.text.Length >= 1)
        {
            PhotonNetwork.CreateRoom(roomInputField.text,
                                    new RoomOptions()
                                    {
                                        MaxPlayers = 4,
                                        BroadcastPropsChangeToAll = true,
                                        DeleteNullProperties = true
                                    }
                                    );
        }
    }

    //////// Photon Room Methods ///////

    public override void OnJoinedRoom()
    {
        //Set ready to false by default on join
        Debug.Log("ON JOINED ROOM");
        Hashtable hash = new Hashtable();
        hash.Add("ready", false);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("left")
                && (bool)PhotonNetwork.LocalPlayer.CustomProperties["left"])
        {
            lobbyPanel.SetActive(false);
            roomPanel.SetActive(true);
            PhotonNetwork.LocalPlayer.CustomProperties["left"] = null;
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            lobbyPanel.SetActive(true);
            roomPanel.SetActive(false);
        }

        foreach (KeyValuePair<int, Player> info in PhotonNetwork.CurrentRoom.Players)
            if (!info.Value.IsLocal)
                OnPlayerEnteredRoom(info.Value);

        roomName.text = PhotonNetwork.CurrentRoom.Name;

    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("ON PLAYER ENTERED ROOM");
        AddPlayerToDictionary(newPlayer);
        UpdatePlayerList(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("ON PLAYER LEFT ROOM");
        DeletePlayerFromDictionary(otherPlayer);
    }

    public void JoinRoom(string roomName)
    {
        Debug.Log("JOIN ROOM");
        PhotonNetwork.JoinRoom(roomName);
    }

    public override void OnLeftRoom()
    {
        Debug.Log("ON LEFT ROOM");
        roomPanel.SetActive(true);
        lobbyPanel.SetActive(false);
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("ready"))
        {
            PhotonNetwork.LocalPlayer.CustomProperties["ready"] = null;
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (Time.time >= nextUpdateTime)
        {
            UpdateRoomList(roomList);
            nextUpdateTime = Time.time + timeBetweenUpdates;
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("ON CONNECTED TO ROOM MASTER");
        PhotonNetwork.JoinLobby();
    }
}
