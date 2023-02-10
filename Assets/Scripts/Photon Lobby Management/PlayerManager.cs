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

    private int kills = 0;
    private int deaths = 0;
    [SerializeField] private int killGoal;
    private bool gameOver = false;

    private addPlayersToFollow targetGroup;

    private HealthHUDManager healthHUDManager;

    private void Awake()
    {
        spawnPointParent = GameObject.FindWithTag("SpawnPoints");
        PV = GetComponent<PhotonView>();
        foreach (Transform point in spawnPointParent.transform)
        {
            spawnPoints.Add(point);
        }

        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("killsToWin"))
        {
            killGoal = (int)PhotonNetwork.LocalPlayer.CustomProperties["killsToWin"];
        }

        Hashtable hash = new Hashtable();
        hash.Add("kills", kills);
        hash.Add("deaths", deaths);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        targetGroup = GameObject.FindGameObjectWithTag("targetGroup").GetComponent<addPlayersToFollow>();

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
        if (gameOver)
        {
            return;
        }
        int randomNumber = Random.Range(0, spawnPoints.Count);
        Transform spawnPoint = spawnPoints[randomNumber];

        GameObject playerToSpawn = playerPrefabs[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]];
        controller = PhotonNetwork.Instantiate(playerToSpawn.name, spawnPoint.position, Quaternion.identity, 0, new object[] { PV.ViewID });

        healthHUDManager = FindObjectOfType<HealthHUDManager>();
        if (healthHUDManager != null) 
        {
            PV.RPC("RPC_CreateHealthItem", RpcTarget.All);
        }

        PV.RPC(nameof(RPC_UpdateCamera), RpcTarget.All);

    }

    [PunRPC]
    private void RPC_CreateHealthItem(PhotonMessageInfo info)
    {
        
        healthHUDManager.AddHealthItem(info.Sender.NickName, info.Sender.ActorNumber);
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
    void RPC_UpdateCamera()
    {
        targetGroup.changeMembers(GameObject.FindGameObjectsWithTag("Player"));
    }


    [PunRPC]
    void RPC_GetKill()
    {
        kills++;

        Hashtable hash = new Hashtable();
        hash.Add("kills", kills);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

        if (kills >= killGoal)
        {
            PV.RPC(nameof(RPC_EndGame), RpcTarget.All);
        }
    }

    [PunRPC]
    void RPC_EndGame()
    {
        gameOver = true;
        StartCoroutine(EndGameCoroutine());
    }

    public static PlayerManager Find(Player player)
    {
        return FindObjectsOfType<PlayerManager>().SingleOrDefault(x => x.PV.Owner == player);
    }

    private IEnumerator EndGameCoroutine()
    {
        if (controller)
        {
            controller.GetComponent<playerController>().DisableInput();
        }
        CanvasGroup gameOverText = GameObject.FindWithTag("GameEndSplashText").GetComponent<CanvasGroup>();
        gameOverText.alpha = 1;
        yield return new WaitForSeconds(3f);
        gameOverText.alpha = 0;
        PhotonNetwork.LoadLevel("Lobby");
    }
}
