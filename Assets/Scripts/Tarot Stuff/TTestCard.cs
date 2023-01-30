using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class TTestCard : TarotCard
{
    public override void Effect(Player player)
    {
        PV.RPC("RPC_CallEffect", player);
    }

    [PunRPC]
    public void RPC_CallEffect()
    {
        Debug.Log("Horseshit");
    }
}
