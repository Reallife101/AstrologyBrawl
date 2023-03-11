using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Random = System.Random;

public class spawnMultipleProjectiles : Ability
{
    private Action method;
    private PhotonView pv;

    [SerializeField] GameObject player;

    [SerializeField] private List<GameObject> objects = new List<GameObject>();
    [SerializeField] private Transform spawnPoint;


    private void Start()
    {
        method = SequentialSpawning;
        pv = GetComponent<PhotonView>();
    }

    public override void Use()
    {
        if (pv.IsMine)
        {
            method();
        }
    }
    public void SpawnSequential()
    {
        method = SequentialSpawning;
    }


    private void SequentialSpawning()
    {

        foreach (GameObject go in objects)
        {
            GameObject game_object = PhotonNetwork.Instantiate(go.name, spawnPoint.position, Quaternion.identity);

            //If spawned object has doDamage and spawner has a photon view
            doDamage dm = game_object.GetComponent<doDamage>();

            if (pv && dm)
            {
                dm.ownerID = pv.GetInstanceID();

                if (transform.parent != null)
                {
                    dm.SetSender(transform.parent.gameObject);
                }
                else
                {
                    dm.SetSender(gameObject);
                }
            }

            // If the character faces the other direction, flip the scale and right transform
            if (player.transform.localScale.x < 0)
            {
                game_object.transform.right = -transform.right.normalized;
                Vector3 proScale = game_object.transform.localScale;
                // pro.transform.localScale = new Vector3(proScale.x, proScale.y, proScale.z);
            }
        }
    }

}
