using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class giveRBVelocity : MonoBehaviour
{
    public float speed;

    private void Start()
    {
        GetComponent<Rigidbody2D>().velocity = transform.right * speed;
    }
}
