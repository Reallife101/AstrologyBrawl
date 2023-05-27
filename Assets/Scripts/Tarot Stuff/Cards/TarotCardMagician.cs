using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TarotCardMagician : TarotCard
{
    private float delay = 3.5f;
    private void Start()
    {
        cardName = CardNames.MAGICIAN;
    }

    public override void Effect(int actorNumber)
    {
        //Debug.Log("doing magician");
        doTo(false, true, actorNumber);
    }

    protected override void doEffect(int actorNumber)
    {
        Player p = PhotonNetwork.CurrentRoom.GetPlayer(actorNumber);
        PlayerManager pm = PlayerManager.Find(p);
        playerController controller = pm.GetPlayerController();
        controller.MagicianDisable(delay);
    }

}
