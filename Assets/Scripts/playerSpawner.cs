using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class playerSpawner : MonoBehaviour
{
    public GameObject playerManagerPrefab;

    private void Start()
    {
        PhotonNetwork.Instantiate(playerManagerPrefab.name, Vector3.zero, Quaternion.identity);
    }
}
