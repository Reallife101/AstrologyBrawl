using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;

public class geminiToss : Ability
{
    [SerializeField] private GameObject littleTwin;
    [SerializeField] private Vector2 forceVector;
    [SerializeField] private float MaxDistance;
    [SerializeField] private float returnSpeed;
    [SerializeField] private float reattachDistance;
    [SerializeField] private PhotonView pv;

    private bool isAttached;
    private bool isReturning;
    private Rigidbody2D rb;
    private Vector3 playerPosition;

    public override void Use()
    {
        //If little guy is returning, ability does nothing
        if (isReturning)
        {
            return;
        }

        if (isAttached)
        {

            if (pv.IsMine)
            {
                pv.RPC("toss", RpcTarget.All);
            }

        }
        else
        {
            //littleTwin.transform.SetParent(transform, true);
            if (pv.IsMine)
            {
                pv.RPC("bringBack", RpcTarget.All);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        isAttached = true;
        isReturning = false;
        rb = littleTwin.GetComponent<Rigidbody2D>();
        littleTwin.transform.position = transform.position;
        //no idea why this fixed it but it did
        littleTwin.transform.SetParent(null, true);
        littleTwin.transform.SetParent(transform, true);
    }

    // Update is called once per frame
    void Update()
    {

        if (isReturning)
        {
            Vector3 diffVector =  transform.position - littleTwin.transform.position;

            UnityEngine.Debug.Log("Distance: "+Vector3.Distance(transform.position, littleTwin.transform.position));

            //if we are close enough, stop returning
            if (Vector3.Distance(transform.position, littleTwin.transform.position) < reattachDistance)
            {
                rb.velocity = Vector2.zero;
                isReturning = false;
                isAttached = true;
                pv.RPC("rebind", RpcTarget.All);

            }
            else
            {
                // Move little twin towards big twin
                rb.velocity = (new Vector2(diffVector.x, diffVector.y)).normalized * returnSpeed;
            }

        }
        else if (!isAttached && Vector2.Distance(littleTwin.transform.position, playerPosition)>MaxDistance)
        {
            rb.velocity = Vector2.zero;

        }
       
    }

    [PunRPC]
    void rebind()
    {
        littleTwin.transform.position = transform.position;
        littleTwin.transform.SetParent(transform, true);
    }

        [PunRPC]
    void bringBack()
    {
        littleTwin.transform.SetParent(null, true);
        isAttached = false;
        isReturning = true;
    }

    [PunRPC]
    void toss()
    {
        littleTwin.transform.SetParent(null, true);
        isAttached = false;

        if (transform.localScale.x <= 0)
        {
            rb.velocity = new Vector2(-forceVector.x, forceVector.y);
        }
        else
        {
            rb.velocity = forceVector;
        }
        playerPosition = transform.position;
    }


}
