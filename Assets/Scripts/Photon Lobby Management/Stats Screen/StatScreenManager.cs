using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.TextCore.Text;
using System;
using System.Reflection;

public class StatScreenManager : MonoBehaviourPunCallbacks
{
    [Serializable]
    public struct Avatar 
    {
        public Sprite splashArt;
        public Sprite avatarFrame;
        public Sprite leftBolt;
        public Sprite rightBolt;
        public Sprite centerBolt;
    }
    [SerializeField]
    private List<GameObject> playerStatsItemsList = new List<GameObject>();

    [SerializeField]
    private List<Avatar> playerAvatarsInfo = new List<Avatar>();

    [SerializeField]
    private GameObject StatsView;

    [SerializeField]
    public GameObject nextGameButton;
    //Winner Stuff
    [SerializeField]
    private TMP_Text winnerText;
    [SerializeField]
    private Image winnerImg;
    //Bolts
    [SerializeField]
    private Image leftBolt;
    [SerializeField]
    private Image rightBolt;
    [SerializeField]
    private Image centerBolt;


    //Player Stuff GameObjects

    [SerializeField]
    private Image statsCharacter;

    [SerializeField]
    private TMP_Text killsText;

    [SerializeField]
    private TMP_Text deathsText;

    [SerializeField]
    private TMP_Text statsName;
    
    private void Start()
    {
        int playerItemIndex = 0;

        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            Debug.Log(player.Value.CustomProperties["kills"].ToString());
            Debug.Log(player.Value.CustomProperties["killsToWin"].ToString());
            if (player.Value.CustomProperties["kills"].ToString() == player.Value.CustomProperties["killsToWin"].ToString())
            {
                int index = (int)player.Value.CustomProperties["playerAvatar"];
                Debug.Log(index);
                if (!playerAvatarsInfo[index].splashArt)
                {
                    Debug.Log("THE STATS SCREEN FOR THIS CHARACTER HAS NOT BEEN SET UP YET (MISSING SPLASH ART)");
                    continue;
                }
                
                winnerText.text = player.Value.NickName;
                winnerImg.sprite = playerAvatarsInfo[index].splashArt;
                leftBolt.sprite = playerAvatarsInfo[index].leftBolt;
                rightBolt.sprite = playerAvatarsInfo[index].rightBolt;
                centerBolt.sprite = playerAvatarsInfo[index].centerBolt;
            }
            else
            {
                Image avatarImage = playerStatsItemsList[playerItemIndex].GetComponent<Image>();
                avatarImage.sprite = playerAvatarsInfo[(int)player.Value.CustomProperties["playerAvatar"]].avatarFrame;
                playerStatsItemsList[playerItemIndex].SetActive(true);
                ++playerItemIndex;
            }
        }

    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount >= 1)
        {
            nextGameButton.SetActive(true);
        }
        else
        {
            nextGameButton.SetActive(false);
        }
    }

    void UpdatePlayerList()
    {
        // Destroy old room items and clear list

        foreach(GameObject go in playerStatsItemsList)
            go.SetActive(false);

        if (PhotonNetwork.CurrentRoom == null)
        {
            return;
        }

        int index = 0; //index of the PlayerList

        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            if (player.Value.CustomProperties["kills"].ToString() != player.Value.CustomProperties["killsToWin"].ToString())
            {
                Image avatarImage = playerStatsItemsList[index].GetComponent<Image>();
                avatarImage.sprite = playerAvatarsInfo[(int)player.Value.CustomProperties["playerAvatar"]].avatarFrame;
                playerStatsItemsList[index].SetActive(true);
                ++index;
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("STATS SCREEN ENTERING ROOM");
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("STATS SCREEN LEAVING ROOM");
        UpdatePlayerList();
    }

    public void OnClickViewStats()
    {
        Debug.Log("LOADING STATS VIEW");
        StatsView.SetActive(true);
        statsName.text = PhotonNetwork.LocalPlayer.NickName;
        statsCharacter.sprite = playerAvatarsInfo[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]].splashArt;
        killsText.text = PhotonNetwork.LocalPlayer.CustomProperties["kills"].ToString();
        deathsText.text = PhotonNetwork.LocalPlayer.CustomProperties["deaths"].ToString();
    }

    public void OnClickNextGame()
    {        
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("FINAL LOBBY SELECTION");
        }
    }
}
