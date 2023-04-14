using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TarotCardMagician : TarotCard
{
    [SerializeField] private float delay;
    private static float teleportDelay = .4f;
    private Vector3[] teleportPoints;
    private void Start()
    {
        cardName = CardNames.MAGICIAN;
    }

    public override void Effect(int actorNumber)
    {
        doTo(false, true, actorNumber);
    }

    protected override void doEffect(int actorNumber)
    {
        Player p = PhotonNetwork.CurrentRoom.GetPlayer(actorNumber);
        PlayerManager pm = PlayerManager.Find(p);
        playerController controller = pm.GetPlayerController();
        controller.magicianDisable(delay);
    }

}
