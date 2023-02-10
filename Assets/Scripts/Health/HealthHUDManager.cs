using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class HealthHUDManager : MonoBehaviour
{
    [SerializeField] private GameObject HealthItemPrefab;
    [SerializeField] private Transform HUDTransform;

    public void AddHealthItem(string nickname, int actornum)
    {
        HealthItem item = Instantiate(HealthItemPrefab, HUDTransform).GetComponent<HealthItem>();
        item.Initialize(nickname, actornum);
    }
}
