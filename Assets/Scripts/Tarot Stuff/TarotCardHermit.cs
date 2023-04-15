using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TarotCardHermit : TarotCard
{
    private float delay = 3.5f;
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
        Debug.Log("doing hermit");
        doTo(false, true, actorNumber);
    }

    protected override void doEffect(int actorNumber)
    {
        Debug.Log("doing hermit to " + actorNumber);
        Player p = PhotonNetwork.CurrentRoom.GetPlayer(actorNumber);
        PlayerManager pm = PlayerManager.Find(p);
        playerController controller = pm.GetPlayerController();
        Debug.Log("controller: " + controller);
        Debug.Log("controller call for hermit");
        controller.hermitDisable(delay);
        Debug.Log("hermit done");
    }
}
