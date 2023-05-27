using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class YankToEnemy : MonoBehaviour
{
    private PhotonView pv;
    public int ownerID;

    private Rigidbody2D playerRB;
    private LineRenderer lr;

    [SerializeField] float YankForce;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();

        playerRB = GetComponentInParent<Rigidbody2D>();
        lr = GetComponent<LineRenderer>();

        lr.startColor = Color.yellow;
        lr.endColor = Color.yellow;
    }

    private void Update()
    {
        if (transform.parent)
        {
            lr.SetPosition(0, transform.parent.position);
            lr.SetPosition(1, transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!pv || !pv.IsMine || collision.gameObject.GetComponent<PhotonView>()?.GetInstanceID() == ownerID)
            return;

        Rigidbody2D playerRB = GetComponentInParent<Rigidbody2D>();
        virgoYankSupport vys = GetComponentInParent<virgoYankSupport>();
        vys.VirgoMixupOnHitAnimTrigger();

        Vector2 VecToEnemy = collision.transform.position - playerRB.transform.position;

        playerRB.AddForce(YankForce * VecToEnemy, ForceMode2D.Impulse);
    }
}
