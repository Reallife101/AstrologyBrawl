using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CountdownManager : MonoBehaviourPunCallbacks
{

    private void Update()
    {
        if (GameObject.FindGameObjectsWithTag("Player").Length == PhotonNetwork.CurrentRoom.PlayerCount)
        {
            Debug.Log(GameObject.FindGameObjectsWithTag("Player").Length + " : " + PhotonNetwork.CurrentRoom.PlayerCount);
            Countdown();
        }

    }

    void Countdown()
    {
        // countdown

        Destroy(gameObject);
    }
}
