using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class tarotSpawnManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> tarotCards;

    private PhotonView PV;
    private List<Transform> spawnPoints = new List<Transform>();

    void Start()
    {
        GameObject spawnPointParent = GameObject.FindWithTag("SpawnPoints");
        PV = GetComponent<PhotonView>();
        foreach (Transform point in spawnPointParent.transform)
        {
            spawnPoints.Add(point);
        }
    }

    // Update is called once per frame
    void Update()
    {
       if (Input.GetKeyDown(KeyCode.M))
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

            GameObject tarotCard = tarotCards[Random.Range(0, tarotCards.Count)];
            PhotonNetwork.Instantiate(tarotCard.name, spawnPoint.position, Quaternion.identity);

        }
    }
}
