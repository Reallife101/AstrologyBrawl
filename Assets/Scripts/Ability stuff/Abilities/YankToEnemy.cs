using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class YankToEnemy : MonoBehaviour
{
    private PhotonView pv;

    public int ownerID;

    [SerializeField] float YankForce;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!pv || !pv.IsMine || collision.gameObject.GetComponent<PhotonView>()?.GetInstanceID() == ownerID)
            return;

        Rigidbody2D myRB = GetComponentInParent<Rigidbody2D>();

        Vector2 VecToEnemy = collision.transform.position - myRB.transform.position;

        myRB.AddForce(YankForce * VecToEnemy, ForceMode2D.Impulse);
    }
}
