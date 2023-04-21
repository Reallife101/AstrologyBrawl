using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TarotDevil : TarotCard
{
    private float delay = 6.66f;
    private float multiplier = 6.66f;
    // Start is called before the first frame update
    void Start()
    {
        cardName = CardNames.JUSTICE;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Effect(int actorNumber)
    {
        doTo(true, false, actorNumber);
    }

    protected override void doEffect(int actorNumber)
    {
        Player p = PhotonNetwork.CurrentRoom.GetPlayer(actorNumber);
        PlayerManager pm = PlayerManager.Find(p);
        playerController controller = pm.GetPlayerController();
        controller.gameObject.GetComponent<DamageManager>().affectAllDamage(multiplier, delay, false, true);
    }
}
