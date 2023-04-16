using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class pickUpCard : MonoBehaviour
{
    private TarotCard t;
    private PhotonView pv;


    void Start()
    {
        t = GetComponent<TarotCard>();
        pv = GetComponent<PhotonView>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<playerController>() != null)
        {

            if (pv.IsMine)
            {
                Player p = collision.gameObject.GetComponent<PhotonView>().Owner;
                t.Effect(p.ActorNumber);
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}