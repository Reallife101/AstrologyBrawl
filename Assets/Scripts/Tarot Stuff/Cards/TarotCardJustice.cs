
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TarotCardJustice : TarotCard
{
    private void Start()
    {
        cardName = CardNames.JUSTICE;
    }

    public override void Effect(int actorNumber)
    {
        doTo(true, true, actorNumber);
    }

    protected override void doEffect(int actorNumber)
    {
        Player p = PhotonNetwork.CurrentRoom.GetPlayer(actorNumber);
        PlayerManager pm = PlayerManager.Find(p);
        playerController controller = pm.GetPlayerController();
        controller.DoJustice();
    }
}
