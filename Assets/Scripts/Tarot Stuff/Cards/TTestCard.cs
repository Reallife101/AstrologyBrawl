using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class TTestCard : TarotCard
{
    [SerializeField] private float moveSpeedDecreaseFactor = 1f;

    public override void Effect(int actorNumber)
    {
        Debug.Log("wOEW ozers ?R !");

        // checking all possible photon views of the given player's actor number for a playerController
        for (int viewId = actorNumber * PhotonNetwork.MAX_VIEW_IDS + 1; viewId < (actorNumber + 1) * PhotonNetwork.MAX_VIEW_IDS; viewId++)
        {
            PhotonView photonView = PhotonView.Find(viewId);
            if (photonView)
            {
                playerController pc = photonView.gameObject.GetComponent<playerController>();
                if (pc)
                {
                    pc.MoveSpeed = pc.MoveSpeed * moveSpeedDecreaseFactor;
                    break;
                }
            }
        }
    }

    protected override void doEffect(int actorNumber)
    {
        throw new System.NotImplementedException();
    }


}
