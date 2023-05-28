using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.InputSystem;

public class KillConditionItem : MonoBehaviourPunCallbacks
{
    public GameObject leftArrowButton;
    public GameObject rightArrowButton;

    ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
    public TMP_Text kill_num_text;
    public int minKills;
    public int maxKills;
    public int killIncrement;
    private int currentKillNum;


    private PlayerControllerInputAsset input;
    private InputAction increase;
    private InputAction decrease;


    private void Awake()
    {
        currentKillNum = minKills;
        kill_num_text.text = currentKillNum.ToString();
        ApplyChangesToAll();

        input = new PlayerControllerInputAsset();
        increase = input.UI.Increase;
        decrease = input.UI.Decrease;

        increase.started += Increase =>
        {
            OnClickRight();
        };

        decrease.started += Decrease =>
        {
            OnClickLeft();
        };

        increase.Enable();
        decrease.Enable();
    }


    public void Start()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("killsToWin"))
        {
            kill_num_text.text = PhotonNetwork.LocalPlayer.CustomProperties["killsToWin"].ToString();
            currentKillNum = (int)PhotonNetwork.LocalPlayer.CustomProperties["killsToWin"];
            ApplyChangesToAll();
        }
    }

    public override void OnJoinedRoom()
    {

        ApplyChangesToAll();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        leftArrowButton.SetActive(false);
        rightArrowButton.SetActive(false);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        ApplyChangesToAll();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        ApplyChangesToAll();
    }

    public void ApplyChangesToAll()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            leftArrowButton.SetActive(true);
            rightArrowButton.SetActive(true);
            //Kinda cursed but the master sets the kill win condition for everyone
            foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
            {
                playerProperties["killsToWin"] = currentKillNum;
                player.Value.SetCustomProperties(playerProperties);
            }
        }
        else
        {
            leftArrowButton.SetActive(false);
            rightArrowButton.SetActive(false);
        }

    }

    public void OnClickLeft()
    {
        if (currentKillNum <= minKills)
        {
            currentKillNum = maxKills;
        }
        else
        {
            currentKillNum -= killIncrement;
        }
        kill_num_text.text = currentKillNum.ToString();
        ApplyChangesToAll();
    }

    public void OnClickRight()
    {
        if (currentKillNum >= maxKills)
        {
            currentKillNum = minKills;
        }
        else
        {
            currentKillNum += killIncrement;
        }
        kill_num_text.text = currentKillNum.ToString();
        ApplyChangesToAll();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (targetPlayer == PhotonNetwork.LocalPlayer && targetPlayer.CustomProperties.ContainsKey("killsToWin"))
        {
            kill_num_text.text = targetPlayer.CustomProperties["killsToWin"].ToString();
            currentKillNum = (int)targetPlayer.CustomProperties["killsToWin"];
        }
    }    

}
