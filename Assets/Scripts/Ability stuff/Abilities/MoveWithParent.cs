using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithParent : MonoBehaviour
{
    // Same as giveRBVelocity but for projectiles that are children of a character

    public float xspeed;
    public float yspeed;

    Rigidbody2D myRB;

    Vector2 displacement_from_parent;
    Vector2 vel;

    private void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        if (transform.parent)
        {
            displacement_from_parent = transform.position - transform.parent.position;
            vel = xspeed * (Vector2)transform.right + yspeed * Vector2.up;
        }
    }

    private void FixedUpdate()
    {
        if (transform.parent)
        {
            // Convert velocity in units/second to units/fixed frame
            displacement_from_parent += vel * Time.fixedDeltaTime;
            myRB.MovePosition((Vector2)transform.parent.position + displacement_from_parent);
        }
    }
}
