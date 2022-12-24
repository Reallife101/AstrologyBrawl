using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using System.IO;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerManager : MonoBehaviour
{
    //Player manager will keep track of scoring, tarot cards, and respawning for the player (ie all data we want to persist past player death)
    private PhotonView PV;
    public GameObject[] playerPrefabs;
    private GameObject spawnPointParent;
    private List<Transform> spawnPoints = new List<Transform>();

    GameObject controller;

    private int kills;
    private int deaths;


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
        if (PV.IsMine)
        {
            Spawn();
        }
    }

    //Spawns/Respawn player
    private void Spawn()
    {
        int randomNumber = Random.Range(0, spawnPoints.Count);
        Transform spawnPoint = spawnPoints[randomNumber];

        GameObject playerToSpawn = playerPrefabs[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]];
        controller = PhotonNetwork.Instantiate(playerToSpawn.name, spawnPoint.position, Quaternion.identity, 0, new object[] { PV.ViewID });
    }

    public void Die()
    {
        //Deaths can be tracked locally
        PhotonNetwork.Destroy(controller);
        Spawn();

        deaths++;

        Hashtable hash = new Hashtable();
        hash.Add("deaths", deaths);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    public void GetKill()
    {
        //But we need to tell our killer that they killed us
        PV.RPC(nameof(RPC_GetKill), PV.Owner);
    }

    [PunRPC]
    void RPC_GetKill()
    {
        kills++;

        Hashtable hash = new Hashtable();
        hash.Add("kills", kills);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    public static PlayerManager Find(Player player)
    {
        return FindObjectsOfType<PlayerManager>().SingleOrDefault(x => x.PV.Owner == player);
    }
}
