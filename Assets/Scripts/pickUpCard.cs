using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class pickUpCard : MonoBehaviour
{
    private TarotCard t;
    private PhotonView pv;

    [SerializeField] int tarotNumber;


    void Start()
    {
        t = GetComponent<TarotCard>();
        pv = GetComponent<PhotonView>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<playerController>() != null)
        {
            Player p = collision.gameObject.GetComponent<PhotonView>().Owner;
            if (pv.IsMine)
            {
                t.Effect(p.ActorNumber);
                pv.RPC("showcard", RpcTarget.All);
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    [PunRPC]
    public void showcard()
    {
        GameObject.FindGameObjectWithTag("tarotManager").GetComponent<TarotDisplayHUDManager>().showTarot(tarotNumber);
    }

}
