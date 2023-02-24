using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class giveRBVelocity : MonoBehaviour
{
    public float xspeed;
    public float yspeed;

    private void Start()
    {
        GetComponent<Rigidbody2D>().velocity = transform.right * xspeed+ new Vector3(0f, yspeed, 0f);
    }
}
