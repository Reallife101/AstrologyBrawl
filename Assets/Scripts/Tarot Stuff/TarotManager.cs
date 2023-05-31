using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using Mono.Cecil;

public class TarotManager : MonoBehaviourPunCallbacks
{
    private PhotonView PV;
    private TarotDisplayHUDManager tarotDisplayHUDManager;
    [SerializeField] private TarotCard[] tarotCards;

    private int KillIndex = 0;
    private int MaxKills;


    void Awake()
    {
        PV = GetComponent<PhotonView>();
        tarotDisplayHUDManager = FindObjectOfType<TarotDisplayHUDManager>();

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
            Debug.Log("Math " + (float)NumOfKills + " / " + MaxKills + " % 0.2 = " + ((float)NumOfKills / MaxKills) % 0.2);
            if (NumOfKills != 0 && (((float)NumOfKills / MaxKills) * 10) % 2 == 0)
            {
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
            TarotCard ChosenCard = tarotCards[Random.Range(0, tarotCards.Length)];
            ChosenCard.Effect(ActorNumber);
            tarotDisplayHUDManager.DisplayCard(ChosenCard.GetCardName());
        }
    }

    [PunRPC]
    public void RPC_SetKillGoal(int killGoal)
    {
        MaxKills = killGoal;
    }
}
