using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine;
using System;


public class TarotCardFool : TarotCard
{

    private static float teleportDelay = .4f;
    private Vector3[] teleportPoints;
    private void Start()
    {
        cardName = CardNames.FOOL;
        //remove    if/when teleport points are set up (not just using respawn)
        GameObject[] tpObjects = GameObject.FindGameObjectsWithTag("tpPoints");
        //uncomment if/when teleport points are set up (not just using respawn)
        //GameObject[] tpObjects = GameObject.FindGameObjectsWithTag("FoolTP");

        teleportPoints = new Vector3[tpObjects.Length];
        for (int i = 0; i < tpObjects.Length; ++i) {
            teleportPoints[i] = tpObjects[i].transform.position;
        }
    }

    protected override void doEffect(int actorNumber)
    {


        //something, something, teleport player, wait a bit, teleport, wait, teleport
        Player p = PhotonNetwork.CurrentRoom.GetPlayer(actorNumber);
        StartCoroutine(teleport3Times(p));
    }

    IEnumerator teleport3Times(Player p)
    {
        GameObject controller = PlayerManager.Find(p).GetController();
        //Vector3[] shuffled = shuffleVectorList(teleportPoints);

        controller.transform.position = teleportPoints[0];
        yield return new WaitForSeconds(teleportDelay);
        controller.transform.position = teleportPoints[1];
        yield return new WaitForSeconds(teleportDelay);
        controller.transform.position = teleportPoints[2];
    }

    //input array will have its orders changed
    private static Vector3[] shuffleVectorList(Vector3[] arr)
    {
        System.Random rng = new System.Random();
        
        Vector3[] rArr = new Vector3[3];
        int n = 0;
        while (n < 3)
        {
            int k = rng.Next(n, arr.Length);
            Vector3 value = arr[k];
            rArr[n] = arr[k];
            arr[k] = arr[n];
            arr[n] = value;
            ++n;
        }
        return rArr;
    }

    public override void Effect(int actorNumber)
    {
        doTo(false, true, actorNumber);
    }
}
