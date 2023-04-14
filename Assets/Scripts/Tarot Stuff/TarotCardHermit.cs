using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TarotCardHermit : TarotCard
{
    [SerializeField] private float delay;
    // Start is called before the first frame update
    void Start()
    {
        cardName = CardNames.HERMIT;
    }

    // Update is called once per frame
    void Update()
    {
        
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
        controller.hermitDisable(delay);
    }
}
