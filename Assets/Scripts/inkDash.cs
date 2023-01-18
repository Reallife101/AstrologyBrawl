using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inkDash : Ability
{
    [SerializeField] Vector2 movement;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Collider2D col;
    [SerializeField] float waitTime;

    private Rigidbody2D myRB;

    private void Awake()
    {
        myRB = GetComponent<Rigidbody2D>();
    }
    public override void Use()
    {
        dash();
        //Do Animation stuff
        col.enabled = false;
        StartCoroutine(offAndOn(waitTime));
    }

    private void dash()
    {
        if (transform.localScale.x <=0)
        {
            myRB.AddForce(-movement, ForceMode2D.Impulse);
        }
        else
        {
            myRB.AddForce(movement, ForceMode2D.Impulse);
        }
        
    }

    IEnumerator offAndOn(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        //Do Animation stuff
        col.enabled = true;
        
    }
}
