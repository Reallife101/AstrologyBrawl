using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class taurusLeap : animationAbility
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] Transform player;
    [SerializeField] Vector2 launchVector;
    [SerializeField] float dropSpeed;

    public override void Use()
    {
        base.Use();
        if(player.localScale.x < 0)
        {
            rb.AddForce(new Vector2(-launchVector.x, launchVector.y), ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(launchVector, ForceMode2D.Impulse);
        }
        
    }

    public void Drop()
    {
        rb.AddForce(transform.up * -dropSpeed);
        Debug.Log("Moo");
    }
}
