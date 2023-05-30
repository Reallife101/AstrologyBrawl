using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TarotCardLovers : TarotCard
{
    private float delay = 6.9f;
    private float speedMultiplier = 1.420f;

    private void Start()
    {
        cardName = CardNames.LOVERS;
    }

    public override void Effect(int actorNumber)
    {
        //Debug.Log("doing lovers");
        PhotonView[] controllers = FindObjectsOfType<PhotonView>();
        List<int> actors = new List<int>();
        int aNum;
        //get all actors that didnt pick up the card
        foreach (PhotonView controller in controllers)
        {
            aNum = controller.gameObject.GetComponent<PhotonView>().Owner.ActorNumber;
            if (aNum != actorNumber)
            {
                actors.Add(aNum);
            }
        }
        if (actors.Count > 0)
        {
            // pick a random one and apply invincibility to everyone else
            aNum = actors[UnityEngine.Random.Range(0, actors.Count)];
            doTo(false, true, aNum);
            // then set the target
            doTarget(aNum);
        }
        else
        {
            Debug.Log("forever alone");
            doTarget(actorNumber);
        }
    }

    protected override void doEffect(int actorNumber)
    {
        Player p = PhotonNetwork.CurrentRoom.GetPlayer(actorNumber);
        PlayerManager pm = PlayerManager.Find(p);
        playerController controller = pm.GetPlayerController();
        controller.LoversInvincible(delay);
    }

    private void doTarget(int actorNumber)
    {
        Player p = PhotonNetwork.CurrentRoom.GetPlayer(actorNumber);
        PlayerManager pm = PlayerManager.Find(p);
        playerController controller = pm.GetPlayerController();
        controller.LoversTarget(delay, speedMultiplier);
    }

}
