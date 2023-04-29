using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class geminiToss : Ability
{
    [SerializeField] private GameObject littleTwin;
    [SerializeField] private Vector2 forceVector;
    [SerializeField] private float MaxDistance;

    private bool isAttached;
    private Rigidbody2D rb;
    private Vector3 startPosition;

    public override void Use()
    {
        if (isAttached)
        {
            littleTwin.transform.SetParent(null, true);
            toss();

        }
        else
        {
            littleTwin.transform.SetParent(transform, true);
        }
        isAttached = !isAttached;
    }

    // Start is called before the first frame update
    void Start()
    {
        isAttached = true;
        rb = littleTwin.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!isAttached && Vector2.Distance(littleTwin.transform.position, startPosition)>MaxDistance)
        {
            rb.velocity = Vector2.zero;

        }
       
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

        startPosition = transform.position;
    }


}
