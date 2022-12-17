using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class giveRBVelocity : MonoBehaviour
{
    public float speed;

    Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        rb.velocity = transform.right*speed;
    }
}
