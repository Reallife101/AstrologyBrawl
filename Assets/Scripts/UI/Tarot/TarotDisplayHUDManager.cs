using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEngine.UI;
using static TarotCard;

public class TarotDisplayHUDManager : MonoBehaviourPunCallbacks
{
    private PhotonView view;

    [SerializeField] List<GameObject> tarotDisplays;

    [SerializeField] GameObject displayCard;
    [SerializeField] Transform LayoutGroupTransform;
    [SerializeField] Transform HealthListingTransform;

    [SerializeField] Texture2D[] Images;
    [SerializeField] float StayTime;

    Transform PlayerTarotHolder;

    public void Start()
    {
        view = GetComponent<PhotonView>();
    }

    public void DisplayCard(TarotCard.CardNames cardName)
    {
        view.RPC("RPC_DisplayCard", RpcTarget.All, cardName);
    }

    private IEnumerator ChangeTexture(CardNames cardName)
    {
        yield return new WaitForSeconds(StayTime);

        for (int i = 0; i < PlayerTarotHolder.childCount; i++)
        {
            if (PlayerTarotHolder.GetChild(i).GetComponent<RawImage>().texture == null)
            {
                PlayerTarotHolder.GetChild(i).GetComponent<RawImage>().texture = Images[(int)cardName];
                break;
            }
        }
    }

    [PunRPC]
    public void RPC_DisplayCard(TarotCard.CardNames cardName)
    {
        if (!PlayerTarotHolder)
        {
            for (int i = 0; i < HealthListingTransform.childCount; i++)
            {
                if (HealthListingTransform.GetChild(i).GetComponent<HealthItem>())
                {
                    HealthItem healthItem = HealthListingTransform.GetChild(i).GetComponent<HealthItem>();
                    Debug.Log(view.OwnerActorNr);
                    if (healthItem.GetOwnerActorNumber() == view.OwnerActorNr)
                    {
                        PlayerTarotHolder = healthItem.GetTarotHolder();
                    }
                }
            }
        }

        GameObject Card = Instantiate(displayCard, LayoutGroupTransform.position, LayoutGroupTransform.rotation);
        Card.GetComponent<DisplayTarotCard>().OnInitialize(Images[(int)cardName], StayTime);
        StartCoroutine(ChangeTexture(cardName));
        Card.transform.SetParent(LayoutGroupTransform);
    }

    public void showTarot(int i)
    {
        foreach(GameObject g in tarotDisplays)
        {
            g.SetActive(false);
        }

        tarotDisplays[i].SetActive(true);
        StartCoroutine(Fade(i));
    }

    IEnumerator Fade(int i)
    {
        yield return new WaitForSeconds(3f);
        tarotDisplays[i].SetActive(false);
    }
}
