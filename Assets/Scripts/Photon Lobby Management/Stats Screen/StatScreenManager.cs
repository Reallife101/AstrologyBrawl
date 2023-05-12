using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class StatScreenManager : MonoBehaviourPunCallbacks
{
    List<PlayerStatsItem> playerStatsItemsList = new List<PlayerStatsItem>();
    public GameObject playerStatsItemPrefab;
    public Transform playerStatsItemParent;
    public GameObject nextGameButton;
    public TMP_Text winnerText;

    private void Start()
    {
        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            if (player.Value.CustomProperties["kills"].ToString() == player.Value.CustomProperties["killsToWin"].ToString())
            {
                winnerText.text = player.Value.NickName;
            }
        }

        UpdatePlayerList();
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
        foreach (PlayerStatsItem item in playerStatsItemsList)
        {
            Destroy(item.gameObject);
        }
        playerStatsItemsList.Clear();

        if (PhotonNetwork.CurrentRoom == null)
        {
            return;
        }

        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            PlayerStatsItem newPlayerStatsItem = Instantiate(playerStatsItemPrefab, playerStatsItemParent).GetComponent<PlayerStatsItem>();
            newPlayerStatsItem.SetPlayerInfo(player.Value);

            if (player.Value == PhotonNetwork.LocalPlayer)
            {
                newPlayerStatsItem.ApplyLocalChanges();
            }

            playerStatsItemsList.Add(newPlayerStatsItem);
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

    public void OnClickNextGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("FINAL LOBBY SELECTION");
        }
    }
}
