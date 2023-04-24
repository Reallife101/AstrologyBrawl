using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Photon.Pun;
using UnityEngine;
using Random = System.Random;

public class SpawnObjects : Ability
{
    private Action method;
    private int numberToSpawn;
    private PhotonView pv;

    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();
    [SerializeField] private GameObject _obj;


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

    public void SpawnRandom(int amount) 
    {
        numberToSpawn = amount;
        method = RandomSpawninng;
    }

    private void SequentialSpawning()
    {
 
        foreach (Transform point in spawnPoints)
        {
            GameObject game_object = PhotonNetwork.Instantiate(_obj.name, point.position, Quaternion.identity);
            //Make sure the doDamage is attached to the parent object (aka _obj)
            doDamage dm = game_object.GetComponent<doDamage>();
            
            if (pv && dm) {
                dm.ownerID = pv.GetInstanceID();
                dm.SetSender(gameObject);
            }
        }
    }

    private void RandomSpawninng()
    {
        // This method would be for the Tarot cards cuz I don't think theire abilites are going to be called throught inputs
        /*
         * method = ManualCall();
         * Use();
         */
        Random rnd = new Random();
        int rand_number;
        for (int i = 0; i < numberToSpawn; i++)
        {
            rand_number = rnd.Next(0, spawnPoints.Count - 1);
            Debug.Log(rand_number);
        }

    }


}
