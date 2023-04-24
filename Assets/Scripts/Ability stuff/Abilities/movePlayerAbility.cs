using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movePlayerAbility : Ability
{
    [SerializeField] Vector2 forceVector;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public override void Use()
    {
        if (transform.localScale.x <= 0)
        {
            rb.AddForce(new Vector2(-forceVector.x, forceVector.y), ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(forceVector, ForceMode2D.Impulse);
        }
    }
}
