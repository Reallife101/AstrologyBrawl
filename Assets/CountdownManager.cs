using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CountdownManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMPro.TMP_Text countdownText;

    ExitGames.Client.Photon.Hashtable ht;
    playerController[] playerControllerList;

    double timerIncrementValue;
    double startTime;
    bool startTimer;
    int playerCount = 0;
    bool playersFrozen = true;

    private void FixedUpdate()
    {
        playerControllerList = FindObjectsOfType<playerController>();

        if (playerControllerList.Length > playerCount)
        {
            playerCount = playerControllerList.Length;
            
            // Disable playercontroler to stop movement
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
            // Using PhotonNetwork.Time to sync time across all the clients
            timerIncrementValue = PhotonNetwork.Time - startTime;

            if (timerIncrementValue >= 4)
            {
                Destroy(gameObject);
            }
            else if (timerIncrementValue >= 3)
            {
                countdownText.SetText("FIGHT!");

                //Unfreeze the players
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
        // Using try catch finally because we will not know which client will be the first to call StartCountdownTimer()
        try
        {
            startTime = double.Parse(PhotonNetwork.CurrentRoom.CustomProperties["StartTime"].ToString());
        }
        catch (NullReferenceException)
        {
            ht = new ExitGames.Client.Photon.Hashtable();
            startTime = PhotonNetwork.Time;
            ht.Add("StartTime", startTime);
            PhotonNetwork.CurrentRoom.SetCustomProperties(ht);
        }
        finally
        {
            startTimer = true;
        }
    }
}
