using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class CountdownManager : MonoBehaviourPunCallbacks
{
    [SerializeField] List<GameObject> countdownImages;
    [SerializeField] GameObject loadingScreen;

    ExitGames.Client.Photon.Hashtable ht;
    playerController[] playerControllerList;

    double timerIncrementValue;
    double startTime;
    bool startTimer;
    int playerCount = 0;
    bool playersFrozen = true;
    CountdownAudio CountdownAudio;

    public void Awake()
    {
        CountdownAudio = GetComponent<CountdownAudio>();
    }

    private void Start()
    {
        if (ht == null)
        {
            ht = new ExitGames.Client.Photon.Hashtable();
        }

        if (PhotonNetwork.CurrentRoom.CustomProperties["StartTime"] == null)
        {
            startTime = PhotonNetwork.Time;
            ht.Add("StartTime", -1);
            PhotonNetwork.CurrentRoom.SetCustomProperties(ht);
        }

    }

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

            if (!startTimer && (playerControllerList.Length >= PhotonNetwork.CurrentRoom.PlayerCount))
            {
                StartCountdownTimer();
                CountdownAudio.CallCountSound();
            }
        }
        else if(!startTimer && PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            StartCountdownTimer();
            CountdownAudio.CallCountSound();
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
                ht["StartTime"] = -1;
                PhotonNetwork.CurrentRoom.SetCustomProperties(ht);
                Destroy(gameObject);
            }
            else if (timerIncrementValue >= 3)
            {
                countdownImages[3].SetActive(true);
                countdownImages[2].SetActive(false);
                CountdownAudio.CallSlaySound();

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
                countdownImages[2].SetActive(true);
                countdownImages[1].SetActive(false);
                CountdownAudio.CallOneSound();
            }
            else if (timerIncrementValue >= 1)
            {
                countdownImages[1].SetActive(true);
                countdownImages[0].SetActive(false);
                CountdownAudio.CallTwoSound();
            }
            else if (timerIncrementValue >= 0)
            {
                countdownImages[0].SetActive(true);
                if (loadingScreen)
                {
                    loadingScreen.SetActive(false);
                }
                CountdownAudio.CallThreeSound();
            }
        }
    }



    void StartCountdownTimer()
    {
        if (double.Parse(PhotonNetwork.CurrentRoom.CustomProperties["StartTime"].ToString()) == -1)
        {
            startTime = PhotonNetwork.Time;
            ht["StartTime"] = startTime;
            PhotonNetwork.CurrentRoom.SetCustomProperties(ht);
        }
        else
        {
            startTime = double.Parse(PhotonNetwork.CurrentRoom.CustomProperties["StartTime"].ToString());
        }
        startTimer = true;
    }
}
