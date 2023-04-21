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
            Debug.Log("picked up");
            Player p = collision.gameObject.GetComponent<PhotonView>().Owner;
            Debug.Log("here 1");
            t.Effect(p.ActorNumber);
            Debug.Log("here 2");
            if (pv.IsMine)
            {
                Debug.Log("by correct player");
                PhotonNetwork.Destroy(gameObject);
                Debug.Log("here 3");
            }
        }
    }
}
