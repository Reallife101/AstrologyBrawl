using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class giveRBVelocity : MonoBehaviour
{
    public Vector2 motion;
    private void Awake()
    {
        GetComponent<Rigidbody2D>().velocity = motion;
    }
}
