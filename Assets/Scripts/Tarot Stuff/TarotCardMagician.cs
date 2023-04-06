using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TarotCardMagician : TarotCard
{

    private static float teleportDelay = .4f;
    private Vector3[] teleportPoints;
    private void Start()
    {
        cardName = CardNames.MAGICIAN;
    }

    public override void Effect(int actorNumber)
    {


        //something, something, teleport player, wait a bit, teleport, wait, teleport
        Player p = PhotonNetwork.CurrentRoom.GetPlayer(actorNumber);
        playerController controller = PlayerManager.Find(p).GetPlayerController();
        controller.magicianDisable();
    }

}
