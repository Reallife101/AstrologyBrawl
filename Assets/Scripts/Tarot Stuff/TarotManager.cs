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
    [SerializeField] private int MaxKills;    //going to be replaced with the PlayerManager's maxkills
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
            //Get the number of kills
            targetPlayer.CustomProperties.TryGetValue("kills", out object kills);
            int NumOfKills = (int)kills;

            if ((float)NumOfKills / MaxKills == KillThresholds[KillIndex])
            {
                PV.RPC("RPC_CallEffect", targetPlayer, targetPlayer.ActorNumber);
                KillIndex++;
            }
        }

        //Used for debugging
        /*if (NumOfKills != 0)
        {
            PV.RPC("RPC_CallEffect", targetPlayer, targetPlayer.NickName);
            KillIndex++;
            Debug.Log("NumOfKills is " + NumOfKills);
        }*/
    }

    [PunRPC]
    public void RPC_CallEffect(int ActorNumber, PhotonMessageInfo info)
    {
        if(ActorNumber == info.Sender.ActorNumber)
        {
            //Picks a random tarot card
            int RandomIndex = Random.Range(0, tarotCards.Count);

            PlayerManager.Find(info.Sender);    //can be used to get the player manager we need (probably going to give effect)

            tarotCards[RandomIndex].Effect();

            //Used for debugging
            /*TTestCard t = new TTestCard();
            t.Effect();*/
        }
    }
}
