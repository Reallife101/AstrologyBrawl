using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CountdownManager : MonoBehaviourPunCallbacks
{
    ExitGames.Client.Photon.Hashtable ht;
    double startTime;
    bool startTimer;
    double timerIncrementValue;
    playerController[] playerControllerList;
    int playerCount = 0;
    bool playersFrozen = true;

    [SerializeField] TMPro.TMP_Text countdownText;

    private void FixedUpdate()
    {
        playerControllerList = FindObjectsOfType<playerController>();

        if (playerControllerList.Length > playerCount)
        {
            playerCount = playerControllerList.Length;
            Debug.Log("playerCount: " + playerCount);
            
            foreach (playerController pc in playerControllerList)
            {
                pc.enabled = false;
            }

            if (!startTimer && playerControllerList.Length == PhotonNetwork.CurrentRoom.PlayerCount)
            {
                StartCountdownTimer();
            }
        }

    }

    private void Update()
    {
        if (startTimer)
        {
            timerIncrementValue = PhotonNetwork.Time - startTime;

            if (timerIncrementValue >= 4)
            {
                Destroy(gameObject);
            }
            else if (timerIncrementValue >= 3)
            {
                countdownText.SetText("FIGHT!");

                if (playersFrozen)
                {
                    foreach (playerController pc in playerControllerList)
                    {
                        pc.enabled = true;
                    }

                    playersFrozen = false;
                }
            }
            else if (timerIncrementValue >= 2)
            {
                countdownText.SetText("1");
            }
            else if (timerIncrementValue >= 1)
            {
                countdownText.SetText("2");
            }
            else if (timerIncrementValue >= 0)
            {
                countdownText.SetText("3");
            }
        }
    }



    void StartCountdownTimer()
    {
        Debug.Log("start countdown timer");
        if (PhotonNetwork.IsMasterClient)
        {
            ht = new ExitGames.Client.Photon.Hashtable();
            startTime = PhotonNetwork.Time;
            ht.Add("StartTime", startTime);
            PhotonNetwork.CurrentRoom.SetCustomProperties(ht);
        }
        else
        {
            startTime = double.Parse(PhotonNetwork.CurrentRoom.CustomProperties["StartTime"].ToString());
        }

        startTimer = true;
    }
}
