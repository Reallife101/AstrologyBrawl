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
    [SerializeField] private float MaxPressTime;
    [SerializeField] private float returnSpeed;
    [SerializeField] private float reattachDistance;
    [SerializeField] private PhotonView pv;

    private bool isAttached;
    private bool isReturning;
    private bool isPressed;
    private float pressTime;
    private float currentMaxDistance;
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
                isPressed = true;
                pressTime = 0f;
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
        isPressed = false;
        pressTime = 0f;
        currentMaxDistance = MaxDistance;
        rb = littleTwin.GetComponent<Rigidbody2D>();
        littleTwin.transform.position = transform.position;
        //no idea why this fixed it but it did
        littleTwin.transform.SetParent(null, true);
        littleTwin.transform.SetParent(transform, true);
    }

    public override void release()
    {
        if (isPressed)
        {
            isPressed = false;
            pv.RPC("toss", RpcTarget.All, Mathf.Min(1f, pressTime / MaxPressTime));
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (isPressed)
        {
            pressTime += Time.deltaTime;
        }

        if (isReturning)
        {
            Vector3 diffVector =  transform.position - littleTwin.transform.position;

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
        else if (!isAttached && Vector2.Distance(littleTwin.transform.position, playerPosition)>currentMaxDistance)
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
    void toss(float percent)
    {
        currentMaxDistance = MaxDistance * percent;
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
