using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class HealthHUDManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private string HealthItemString;
    [SerializeField] private Transform HUDTransform;

    private List<HealthItem> HealthItems = new List<HealthItem>();

    public HealthItem AddHealthItem(string nickname, int actornum)
    {
        HealthItem item = PhotonNetwork.Instantiate(HealthItemString, HUDTransform.position, HUDTransform.rotation).GetComponent<HealthItem>();
        item.Initialize(nickname, actornum);
        HealthItems.Add(item);
        return item;
    }

    public HealthItem FindMyHealthItem()
    {
        foreach(HealthItem item in HealthItems)
        {
            //if(//you, the player's, actor number is the same as one stored by a health thing, then this is your health!)
        }
        return null;
    }
}
