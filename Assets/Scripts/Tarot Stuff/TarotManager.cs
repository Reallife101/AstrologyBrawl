using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class TarotManager : MonoBehaviourPunCallbacks
{
    private PhotonView PV;

    private List<TarotCard> tarotCards;

    private int KillIndex = 0;
    [SerializeField] private float MaxKills;    //going to replace with the PlayerManager's maxkills
    [SerializeField] private float[] KillThresholds;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
        tarotCards = new List<TarotCard>();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey("kills"))
        {
            targetPlayer.CustomProperties.TryGetValue("kills", out object kills);
            int NumOfKills = (int)kills;
            //if (NumOfKills / MaxKills == KillThresholds[KillIndex])
            //{
            //int RandomIndex = Random.Range(0, tarotCards.Count);
            //tarotCards[RandomIndex].Effect(targetPlayer);
            
            if(NumOfKills != 0)
            {
                PV.RPC("RPC_CallEffect", targetPlayer);
                KillIndex++;
            }

            //}
        }
    }

    [PunRPC]
    public void RPC_CallEffect()
    {
        TTestCard t = new TTestCard();
        t.Effect();
    }
}
