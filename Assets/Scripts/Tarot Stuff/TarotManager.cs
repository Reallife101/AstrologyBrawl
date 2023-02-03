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
        TTestCard t = new TTestCard();
        tarotCards.Add(t);
        
        //Sets the kill goal on all TarotManagers
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.IsMasterClient)
            {
                if (player.CustomProperties.ContainsKey("killsToWin"))
                {
                    player.CustomProperties.TryGetValue("killsToWin", out object killsToWin);
                    int killGoal = (int)killsToWin;

                    PV.RPC("RPC_SetKillGoal", RpcTarget.All, killGoal);

                    //TODO: Set the KillThresholds
                }
            }
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey("kills"))
        {
            //Get the number of kills
            targetPlayer.CustomProperties.TryGetValue("kills", out object kills);
            int NumOfKills = (int)kills;

            //OnPlayerProperties updates on initialization, so prevents any tarots from activating at 0
            if (NumOfKills != 0)
            {
                //TODO: Use KillThresholds as another check

                PV.RPC("RPC_CallEffect", targetPlayer, targetPlayer.ActorNumber);
                KillIndex++;
                Debug.Log("NumOfKills is " + NumOfKills);
            }
        }

        
    }

    [PunRPC]
    public void RPC_CallEffect(int ActorNumber, PhotonMessageInfo info)
    {
        if(ActorNumber == info.Sender.ActorNumber)
        {
            //Picks a random tarot card
            tarotCards[Random.Range(0, tarotCards.Count)].Effect(ActorNumber);
        }
    }

    [PunRPC]
    public void RPC_SetKillGoal(int killGoal)
    {
        MaxKills = killGoal;
    }
}
