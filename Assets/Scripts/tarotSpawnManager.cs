using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Threading;

public class tarotSpawnManager : MonoBehaviour
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

    void Start()
    {
        GameObject[] spawnPointObjects = GameObject.FindGameObjectsWithTag("tpPoints");
        PV = GetComponent<PhotonView>();
        foreach (GameObject g in spawnPointObjects)
        {
            spawnPoints.Add(g.transform);
        }

        timer = Random.Range(minSpawnTime, maxSpawnTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <=0 && PV.IsMine)
        {
            spawn();

            timer = timer = Random.Range(minSpawnTime, maxSpawnTime);
        }

        timer -= Time.deltaTime;
    }

    private void spawn()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

        GameObject tarotCard = tarotCards[Random.Range(0, tarotCards.Count)];
        PhotonNetwork.Instantiate(tarotCard.name, spawnPoint.position, Quaternion.identity);
    }
}
