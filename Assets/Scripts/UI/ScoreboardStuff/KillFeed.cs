using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class KillFeed : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform container;
    [SerializeField] private GameObject killFeedItemPrefab;
    public static KillFeed Instance { get; private set; }

    private void Awake()
    {
        //Singleton

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void MakeKillFeed(Player Killer, Player KilledPlayer)
    {
        // Debug.Log("Please tell me this is working"); //me fr
        if(Killer != null && KilledPlayer != null)
        {
            KillFeedItem newItem = Instantiate(killFeedItemPrefab, container).GetComponent<KillFeedItem>();
            newItem.Initialize(Killer, KilledPlayer);
        }
    }
}
