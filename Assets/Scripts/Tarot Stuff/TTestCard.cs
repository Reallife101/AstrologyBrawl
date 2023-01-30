using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class TTestCard : TarotCard
{
    public override void Effect(Player player)
    {
        PV.RPC("RPC_Effect", RpcTarget.All, player);
        //Debug.Log("wOwzer! .< >>> Effect");
    }

    [PunRPC]
    public override void RPC_Effect()
    {

    }
}
