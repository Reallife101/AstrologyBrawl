using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;

public class TarotDisplayHUDManager : MonoBehaviourPunCallbacks
{
    private PhotonView view;

    [SerializeField] GameObject displayCard;
    [SerializeField] Transform LayoutGroupTransform;

    [SerializeField] Texture2D[] Images;
    [SerializeField] float StayTime;

    public void Start()
    {
        view = GetComponent<PhotonView>();
    }

    public void DisplayCard(TarotCard.CardNames cardName)
    {
        Debug.Log("display card called: " + cardName);

        view.RPC("RPC_DisplayCard", RpcTarget.All, cardName);
    }

    [PunRPC]
    public void RPC_DisplayCard(TarotCard.CardNames cardName)
    {
        GameObject Card = Instantiate(displayCard, LayoutGroupTransform.position, LayoutGroupTransform.rotation);
        Card.GetComponent<DisplayTarotCard>().OnInitialize(Images[(int)cardName], StayTime);
        Card.transform.SetParent(LayoutGroupTransform);
    }
}
