using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class StateManager : MonoBehaviour
{
    public enum States
    {
        Idle,
        GroundLight,
        GroundHeavy,
        AirLight,
        AirHeavy,
    }

    [SerializeField] private AttackFrameSO firstLightAttack;
    private AttackFrameSO currentAttack;
    private PhotonView myPV;
    public States currentState { get; private set; }
    [SerializeField] private doDamage hitbox;
    [SerializeField] private Animator playerAnimator;
    //Attack state values
    private float inputBufferTimeRemaining;
    private float attackTimeRemaining;
    


    private void Awake()
    {
        myPV = GetComponent<PhotonView>();
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
                //Reset to idle
                currentState = States.Idle;
                currentAttack = null;
            }
            //And there is input buffered
            else
            {
                //Get the next attack
                AttackFrameSO nextAttack = currentAttack.NextAttackFrameSO;
                //If there's no next in the combo, return to idle
                if (nextAttack == null)
                {
                    currentState = States.Idle;
                    currentAttack = null;
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

    private void UpdateAttackInfo()
    {
        //Starts the countdown for the current attack's duration, changes damage of the hitbox to match the current attack, and starts the corresponding attack anim
        attackTimeRemaining = currentAttack.duration;
        hitbox.SetDamage(currentAttack.damage);
        playerAnimator.SetTrigger(currentAttack.attackAnimationName);
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
            currentAttack = firstLightAttack;
            UpdateAttackInfo();
        }

        if (currentAttack != null)
        {
            inputBufferTimeRemaining = currentAttack.inputBufferTime;
        }
    }
}
