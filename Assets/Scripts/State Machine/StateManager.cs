using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class StateManager : MonoBehaviour
{
    //Combat states
    public enum States
    {
        Idle,
        GroundLight,
        GroundHeavy,
        AirLight,
        AirHeavy,
    }
    //Attacks
    [SerializeField] private AttackFrameSO firstLightGround;
    [SerializeField] private AttackFrameSO firstHeavyGround;
    [SerializeField] private AttackFrameSO firstLightAir;
    [SerializeField] private AttackFrameSO firstHeavyAir;
    private AttackFrameSO currentAttack;
    //Components
    private PhotonView myPV;
    private Rigidbody2D myRB;
    //Aerial variables
    private float originalGravity;
    public States currentState { get; private set; }
    [SerializeField] private doDamage hitbox;
    [SerializeField] private Animator playerAnimator;


    //Attack state values
    private float inputBufferTimeRemaining;
    private float attackTimeRemaining;
    


    private void Awake()
    {
        myPV = GetComponent<PhotonView>();
        myRB = GetComponent<Rigidbody2D>();
        originalGravity = myRB.gravityScale;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!myPV.IsMine || currentState == States.Idle)
        {
            return;
        }

        //Count down
        attackTimeRemaining -= Time.deltaTime;
        inputBufferTimeRemaining -= Time.deltaTime;

        //If the attack is over
        if (attackTimeRemaining <= 0)
        {
            //And there is no input buffered
            if (inputBufferTimeRemaining <= 0)
            {
                ReturnToNeutral();
            }
            //And there is input buffered
            else
            {
                //Get the next attack
                AttackFrameSO nextAttack = currentAttack.NextAttackFrameSO;
                //If there's no next in the combo, return to idle
                if (nextAttack == null)
                {
                    ReturnToNeutral();
                }
                //If there is, begin next attack
                else
                {
                    currentAttack = nextAttack;
                    UpdateAttackInfo();
                }
            }
        }

    }

    private void ReturnToNeutral()
    {
        currentState = States.Idle;
        currentAttack = null;
        myRB.gravityScale = originalGravity;
    }

    private void UpdateAttackInfo()
    {
        //Starts the countdown for the current attack's duration, changes damage of the hitbox to match the current attack, and starts the corresponding attack anim
        attackTimeRemaining = currentAttack.duration;
        hitbox.SetDamage(currentAttack.damage);
        playerAnimator.SetTrigger(currentAttack.attackAnimationName);
        myRB.velocity = Vector2.zero;
        myRB.AddForce(new Vector2(Mathf.Sign(transform.localScale.x) * currentAttack.forwardMovement, 0), ForceMode2D.Impulse);
    }

    //When light attack is pressed, check if grounded and idle, if so start the attack chain. If an attack is in progress, allow input buffer.
   public void LightAttackPressed(bool isGrounded)
    {
        if (!myPV.IsMine)
        {
            return;
        }

        if(currentState == States.Idle && isGrounded)
        {

            currentState = States.GroundLight;
            currentAttack = firstLightGround;
            UpdateAttackInfo();
        }

        else if (currentState == States.Idle && !isGrounded)
        {
            myRB.gravityScale = 0;
            currentState = States.AirLight;
            currentAttack = firstLightAir;
            UpdateAttackInfo();
        }

        if (currentAttack != null && currentState == States.GroundLight || currentState == States.AirLight)
        {
            inputBufferTimeRemaining = currentAttack.inputBufferTime;
        }
    }

    public void HeavyAttackPressed(bool isGrounded)
    {
        if (!myPV.IsMine)
        {
            return;
        }

        if (currentState == States.Idle && isGrounded)
        {
            currentState = States.GroundHeavy;
            currentAttack = firstHeavyGround;
            UpdateAttackInfo();
        }

        else if (currentState == States.Idle && !isGrounded)
        {
            myRB.gravityScale = 0;
            currentState = States.AirHeavy;
            currentAttack = firstHeavyAir;
            UpdateAttackInfo();
        }

        if (currentAttack != null & currentState == States.GroundHeavy || currentState == States.AirHeavy)
        {
            inputBufferTimeRemaining = currentAttack.inputBufferTime;
        }
    }
}
