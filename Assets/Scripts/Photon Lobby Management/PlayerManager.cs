using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using System.IO;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    //Player manager will keep track of scoring, tarot cards, and respawning for the player (ie all data we want to persist past player death)
    private PhotonView PV;
    public GameObject[] playerPrefabs;
    private GameObject spawnPointParent;
    private List<Transform> spawnPoints = new List<Transform>();

    GameObject controller;
    playerController PlayerController;

    private int kills = 0;
    private int deaths = 0;
    [SerializeField] private int killGoal;
    private bool gameOver = false;

    private addPlayersToFollow targetGroup;

    private HealthHUDManager healthHUDManager;
    private HealthItem healthItem;
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

    private void Update()
    {
        if (PV.IsMine)
        {
            healthItem.UpdateCooldown(PlayerController.AbilityOneCurrCooldown, PlayerController.AbilityTwoCurrCooldown);
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
        PlayerController = controller.GetComponent<playerController>();
        healthItem.SetMaxCooldowns(PlayerController.AbilityOneMaxCooldown, PlayerController.AbilityTwoMaxCooldown);
        healthItem.ActivateTimers();

        PV.RPC(nameof(RPC_UpdateCamera), RpcTarget.All);
    }

    public void SetController(GameObject _controller)
    {
        controller = _controller;
    }

    public void UpdateHealth()
    {
        if(PV.IsMine)
        {
            PV.RPC("RPC_UpdateHealthItem", RpcTarget.All);
        }
    }

    [PunRPC]
    private void RPC_ScreenShake(PhotonMessageInfo info)
    {
        CinemachineShake.Instance.ShakeCamera(10f, .15f);
    }

    //Will create new health item if needed and updating playerController with approprate healthItem
    [PunRPC]
    private void RPC_UpdateHealthItem(PhotonMessageInfo info)
    {
        healthHUDManager = FindObjectOfType<HealthHUDManager>();
        if (healthHUDManager != null && healthItem == null)
        {
            healthItem = healthHUDManager.AddHealthItem(info.Sender.NickName, info.Sender.ActorNumber);
        }

        //Sets relevant information for the HealthItems
        Health health = controller.GetComponent<PlayerHealth>();
        health.setHealthItem(healthItem);
        healthItem.SetHealthUI(health.getMaxHealth());
    }

    public void Die()
    {
        //Deaths can be tracked locally
        PhotonNetwork.Destroy(controller);
        Spawn();
        UpdateLeader();
        //Spawns the platform for the player after respawning
        controller.GetComponent<playerController>().SpawnDeathPlatform();
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
        PV.RPC(nameof(SyncKills), RpcTarget.Others, kills);
        PV.RPC("RPC_ScreenShake", RpcTarget.All);
        PV.RPC("RPC_UpdateKillCount", RpcTarget.All, kills);

        Hashtable hash = new Hashtable();
        hash.Add("kills", kills);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

        if (kills >= killGoal)
        {
            PV.RPC(nameof(RPC_EndGame), RpcTarget.All);
        }
    }

    [PunRPC]
    void SyncKills(int newKills)
    {
        kills = newKills;
    }

    [PunRPC]
    void RPC_UpdateKillCount(int killCount)
    {
        healthItem.UpdateKillCount(killCount);
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
        PhotonNetwork.LoadLevel("StatScreen");
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        UpdateLeader();
    }

    private void UpdateLeader()
    {
        bool isKing = true;
        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            if ((int)player.Value.CustomProperties["kills"] > kills)
            {
                isKing = false;
            }
        }
        controller?.GetComponent<playerController>().ToggleCrown(isKing);
    }
}
