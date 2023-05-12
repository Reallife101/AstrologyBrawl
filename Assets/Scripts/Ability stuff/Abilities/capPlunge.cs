using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class capPlunge : Ability
{
    [SerializeField] private StateManager sm;
    [SerializeField] private playerController pc;
    [SerializeField] private Rigidbody2D rb;
    
    [Header("Slide")]
    [SerializeField] private AttackFrameSO groundSlide;
    [SerializeField] private string groundSlideTrigger;
    [SerializeField] private float slideSpeed;

    [Header("Plunge")]
    [SerializeField]
    private string airPlungeTrigger;
    [SerializeField] private AttackFrameSO airPlunge;
    [SerializeField] private float plungeSpeed;

    public override void Use()
    {
        if (pc.isGrounded)
        {
            SlideCast();   
        }

        else
        {
            PlungeCast();
        }
        
    }

    public void PlungeCast()
    {
        playerAnimator.SetTrigger(airPlungeTrigger);
        sm.StartCastingIconic(airPlunge);
    }

    public void Plunge()
    {
        rb.AddForce(new Vector2(rb.velocity.y, -plungeSpeed * 25));
    }

    public void SlideCast()
    {
        playerAnimator.SetTrigger(groundSlideTrigger);
        sm.StartCastingIconic(groundSlide);
        if(transform.localScale.x < 0)
        {
            rb.AddForce(-transform.right * slideSpeed, ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(transform.right * slideSpeed, ForceMode2D.Impulse);
        }
        
    }



}
