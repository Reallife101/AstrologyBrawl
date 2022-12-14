using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerManager : MonoBehaviour
{
    //Player manager will keep track of scoring, tarot cards, and respawning for the player (ie all data we want to persist past player death)
    private PhotonView PV;
    public GameObject[] playerPrefabs;
    private GameObject spawnPointParent;
    private List<Transform> spawnPoints = new List<Transform>();


    private void Awake()
    {
        spawnPointParent = GameObject.FindWithTag("SpawnPoints");
        PV = GetComponent<PhotonView>();
        foreach (Transform point in spawnPointParent.transform)
        {
            spawnPoints.Add(point);
        }
    }
    private void Start()
    {
        Spawn();
    }

    //Spawns/Respawn player
    private void Spawn()
    {
        if (!PV.IsMine)
        {
            return;
        }
        int randomNumber = Random.Range(0, spawnPoints.Count);
        Transform spawnPoint = spawnPoints[randomNumber];

        GameObject playerToSpawn = playerPrefabs[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]];
        PhotonNetwork.Instantiate(playerToSpawn.name, spawnPoint.position, Quaternion.identity, 0, new object[] { PV.ViewID });
    }
}
