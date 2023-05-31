using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Threading;
using System;


public class tarotSpawnManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private List<GameObject> tarotCards;

    [SerializeField]
    private float minSpawnTime;
    [SerializeField]
    private float maxSpawnTime;

    private PhotonView PV;
    private List<Transform> spawnPoints = new List<Transform>();

    private float timer;


    [Serializable]
    public struct TarotInfo
    {
        public string name;
        public GameObject TarotGO;
    }

    [SerializeField] private List<TarotInfo> tarotCardsList = new List<TarotInfo>();

    private Dictionary<string, GameObject> tarotCardsDict = new Dictionary<string, GameObject>();

    [SerializeField] private string[] tarotPool;
    void Start()
    {
        GameObject[] spawnPointObjects = GameObject.FindGameObjectsWithTag("TarotSpawn");
        PV = GetComponent<PhotonView>();
        foreach (GameObject g in spawnPointObjects)
        {
            spawnPoints.Add(g.transform);
        }

        timer = UnityEngine.Random.Range(minSpawnTime, maxSpawnTime);


        foreach (TarotInfo TarotInfo in tarotCardsList)
        {
            try
            {
                tarotCardsDict.Add(TarotInfo.name, TarotInfo.TarotGO);
            }
            catch (Exception e)
            {
                Debug.Log("Tarot card " + TarotInfo.name + " was already added");
            }
        }

        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("TarotCards"))
            photonView.RPC(nameof(RPC_AddCardsToPool), RpcTarget.All, (string[])PhotonNetwork.LocalPlayer.CustomProperties["TarotCards"]);
    }

    [PunRPC]
    void RPC_AddCardsToPool(string[] TarotNames)
    {
        tarotPool = TarotNames;
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        //Debug.Log("Sending Pool");
        if (changedProps.ContainsKey("TarotCards") && PhotonNetwork.LocalPlayer.IsLocal)
            photonView.RPC(nameof(RPC_AddCardsToPool), RpcTarget.All, (string[])PhotonNetwork.LocalPlayer.CustomProperties["TarotCards"]);
    }


    // Update is called once per frame
    void Update()
    {
        if (timer <=0 && PV.IsMine)
        {
            spawn();

            timer = timer = UnityEngine.Random.Range(minSpawnTime, maxSpawnTime);
        }

        timer -= Time.deltaTime;
    }

    private void spawn()
    {
        Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)];

        string name = tarotPool[UnityEngine.Random.Range(0, tarotCards.Count)];
        GameObject tarotCard = tarotCardsDict[name];
        PhotonNetwork.Instantiate(tarotCard.name, spawnPoint.position, Quaternion.identity);
    }
}
