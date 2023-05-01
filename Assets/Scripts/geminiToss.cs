using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class geminiToss : Ability
{
    [SerializeField] private GameObject littleTwin;
    [SerializeField] private Vector2 forceVector;
    [SerializeField] private float MaxDistance;
    [SerializeField] private float returnSpeed;
    [SerializeField] private float reattachDistance;

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
            littleTwin.transform.SetParent(null, true);
            isAttached = false;
            toss();

        }
        else
        {
            //littleTwin.transform.SetParent(transform, true);
            bringBack();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        isAttached = true;
        isReturning = false;
        rb = littleTwin.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        if (isReturning)
        {
            Vector3 diffVector = transform.position - littleTwin.transform.position;

            //if we are close enough, stop returning
            if (diffVector.magnitude < reattachDistance)
            {
                rb.velocity = Vector2.zero;
                isReturning = false;
                isAttached = true;
                littleTwin.transform.SetParent(transform, true);

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

    void bringBack()
    {
        isReturning = true;
    }

    void toss()
    {
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
