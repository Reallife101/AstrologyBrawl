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
    [SerializeField] private int MaxKills;    //going to replace with the PlayerManager's maxkills
    [SerializeField] private float[] KillThresholds;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
        tarotCards = new List<TarotCard>();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        /*if (changedProps.ContainsKey("kills"))
        {
            targetPlayer.CustomProperties.TryGetValue("kills", out object kills);
            float NumOfKills = (float)kills;
            if (NumOfKills / MaxKills == KillThresholds[KillIndex])
            {
                int RandomIndex = Random.Range(0, tarotCards.Count);
                tarotCards[RandomIndex].Effect(targetPlayer);
                KillIndex++;
            }
        }*/
    }
}
