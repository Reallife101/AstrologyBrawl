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
        GroundAttack,
        AirAttack,
        Recovery,
        HitStun,
        Charging,
        Casting,
        Shield,
    }
    //Attacks
    [SerializeField] private AttackFrameSO firstLightGround;
    [SerializeField] private AttackFrameSO firstHeavyGround;
    [SerializeField] private AttackFrameSO firstLightAir;
    [SerializeField] private AttackFrameSO firstHeavyAir;
    private AttackFrameSO currentAttack;
    private States queuedState = States.Idle;
    //Components
    private PhotonView myPV;
    private Rigidbody2D myRB;
    //Aerial variables
    private float originalGravity;
    [SerializeField] private float airAttackGravityModifier;
    public States currentState { get; private set; }
    [SerializeField] private List<doDamage> hitboxes;
    [SerializeField] private Animator playerAnimator;


    //Attack state values
    private float inputBufferTimeRemaining;
    private float attackTimeRemaining;
    private float recoveryTimeLeft;
    private float hitStunTimeLeft;
    private float chargeTimeLeft;
    private DamageManager.AttackStates atk_state;  

    private float lastChargedMultiplier = 1f;

    //Audio
    [Header("Audio Scripts")]
    audioManager audioManager;
    
    private void Awake()
    {
        audioManager = GetComponent<audioManager>();
        myPV = GetComponent<PhotonView>();
        myRB = GetComponent<Rigidbody2D>();
        originalGravity = myRB.gravityScale;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!myPV.IsMine || currentState == States.Idle || currentState == States.Casting || currentState == States.Shield)
        {
            return;
        }

        //If charging, handle charging
        if (currentState == States.Charging)
        {
            chargeTimeLeft -= Time.deltaTime;
            if (chargeTimeLeft <= 0)
            {
                EndCharge();
            }
            return;
        }

        //Count down recovery time, if over return to neutral
        recoveryTimeLeft -= Time.deltaTime;
        if (currentState == States.Recovery)
        {
            if (recoveryTimeLeft <= 0)
            {
                ReturnToNeutral();
            }
            return;
        }

        //If hitstunned, handle hitstun
        hitStunTimeLeft -= Time.deltaTime;
        if (currentState == States.HitStun)
        {
            if (hitStunTimeLeft <= 0)
            {
                playerAnimator.SetBool("HitStun", false);
                ReturnToNeutral();
            }
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
                InitiateRecovery();
            }
            //And there is input buffered
            else
            {
                //Get the next attack
                AttackFrameSO nextAttack = currentAttack.NextAttackFrameSO;
                //If there's no next in the combo, return to idle
                if (nextAttack == null)
                {
                    InitiateRecovery();
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



    private void InitiateRecovery()
    {
        currentState = States.Recovery;
        recoveryTimeLeft = currentAttack.finisherEndlag;
        myRB.velocity = Vector2.zero;
        myRB.gravityScale = originalGravity;
    }

    //Overload for casting
    private void InitiateRecovery(float endlag)
    {
        currentState = States.Recovery;
        recoveryTimeLeft = endlag;
        myRB.velocity = Vector2.zero;
        myRB.gravityScale = originalGravity;
    }

    private void ReturnToNeutral()
    {
        currentState = States.Idle;
        currentAttack = null;
        atk_state = DamageManager.AttackStates.NoDamage;
        lastChargedMultiplier = 1;
    }

    private void UpdateAttackInfo()
    {
        foreach(doDamage doD in hitboxes)
        {
            doD.SetValues(currentAttack.damage, atk_state, currentAttack.hitStunTime, currentAttack.knockbackPower, currentAttack.launchDirection, currentAttack.shakeTime, currentAttack.shakeIntensity, gameObject);
        }
        //Starts the countdown for the current attack's duration, changes damage of the hitbox to match the current attack, and starts the corresponding attack anim
        attackTimeRemaining = currentAttack.duration;
        playerAnimator.SetTrigger(currentAttack.attackAnimationName);
        myRB.velocity = Vector2.zero;
        myRB.AddForce(new Vector2(Mathf.Sign(transform.localScale.x) * currentAttack.forwardMovement, 0), ForceMode2D.Impulse);
    }

    //Overload for charged attacks
    private void UpdateAttackInfo(float chargeMulti)
    {
        foreach (doDamage doD in hitboxes)
        {
            doD.SetValues(currentAttack.damage, atk_state, currentAttack.hitStunTime, currentAttack.knockbackPower, chargeMulti * currentAttack.launchDirection, currentAttack.shakeTime, currentAttack.shakeIntensity, gameObject, chargeMulti);
        }
        //Starts the countdown for the current attack's duration, changes damage of the hitbox to match the current attack, and starts the corresponding attack anim
        attackTimeRemaining = currentAttack.duration;
        //hitbox.SetValues(currentAttack.damage * chargeMulti, currentAttack.hitStunTime, currentAttack.knockbackPower, chargeMulti * currentAttack.launchDirection, gameObject);
        //hitbox.SetValues(atk_state, currentAttack.hitStunTime, currentAttack.knockbackPower, chargeMulti * currentAttack.launchDirection, gameObject, chargeMulti);
        playerAnimator.SetTrigger(currentAttack.attackAnimationName);
        myRB.velocity = Vector2.zero;
        myRB.AddForce(new Vector2(Mathf.Sign(transform.localScale.x) * currentAttack.forwardMovement, 0), ForceMode2D.Impulse);
    }

    private void EnterCharge()
    {
        currentState = States.Charging;
        chargeTimeLeft = currentAttack.chargeTimeAllowed;
        playerAnimator.SetTrigger(currentAttack.freezeFrameName);
        audioManager.CallHeavyAttackCharge();
    }

    public void EndCharge()
    {
        if(currentState == States.Charging)
        {
            float multiplier = Mathf.Lerp(1, currentAttack.chargeMultiplier, (currentAttack.chargeTimeAllowed - chargeTimeLeft) / currentAttack.chargeTimeForMax);
            currentState = queuedState;
            queuedState = States.Idle;
            UpdateAttackInfo(multiplier);
            lastChargedMultiplier = multiplier;
            audioManager.CallHeavyAttackRelease();
        }
    }

    //When light attack is pressed, check if grounded and idle, if so start the attack chain. If an attack is in progress, allow input buffer.
   public void LightAttackPressed(bool isGrounded)
    {
        if (!myPV.IsMine)
        {
            return;
        }

        atk_state = DamageManager.AttackStates.Light;

        if (currentState == States.Idle && isGrounded)
        {
            currentState = States.GroundAttack;
            currentAttack = firstLightGround;
           
            UpdateAttackInfo();
            audioManager.CallLightAttack();
        }

        else if (currentState == States.Idle && !isGrounded)
        {
            myRB.gravityScale = originalGravity * airAttackGravityModifier;
            currentState = States.AirAttack;
            currentAttack = firstLightAir;
            UpdateAttackInfo();
            audioManager.CallLightAttack();
        }

        if (currentAttack != null && currentState == States.GroundAttack || currentState == States.AirAttack)
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

        atk_state = DamageManager.AttackStates.Heavy;

        if (currentState == States.Idle && isGrounded)
        {
            if(firstHeavyGround.chargeTimeAllowed != 0)
            {
                queuedState = States.GroundAttack;
                currentAttack = firstHeavyGround;
                EnterCharge();
            }
            else
            {
                currentState = States.GroundAttack;
                currentAttack = firstHeavyGround;
                audioManager.CallHeavyAttackRelease();
                UpdateAttackInfo();
            }
        }

        else if (currentState == States.Idle && !isGrounded)
        {
            myRB.gravityScale = originalGravity * airAttackGravityModifier;
            currentState = States.AirAttack;
            currentAttack = firstHeavyAir;
            audioManager.CallHeavyAttackRelease();
            UpdateAttackInfo();
        }

        if (currentAttack != null & currentState == States.GroundAttack || currentState == States.AirAttack)
        {
            inputBufferTimeRemaining = currentAttack.inputBufferTime;
        }
    }
    [PunRPC]
    void HitStunned(float hitStunValue)
    {
        inputBufferTimeRemaining = 0;
        attackTimeRemaining = 0;
        recoveryTimeLeft = 0;
        hitStunTimeLeft = hitStunValue;
        myRB.gravityScale = originalGravity;
        currentState = States.HitStun;
        playerAnimator.SetBool("HitStun", true);
        playerAnimator.SetTrigger("HitStunTrigger");
    }

    public void chargedProjectileSetter(doDamage input)
    {
        input.SetValues(currentAttack.damage, atk_state, currentAttack.hitStunTime, currentAttack.knockbackPower, currentAttack.launchDirection, currentAttack.shakeTime, currentAttack.shakeIntensity, gameObject, lastChargedMultiplier);
    }

    public void StartCasting()
    {
        currentState = States.Casting;
    }

    public void StartCastingIconic(AttackFrameSO af)
    {
        currentState = States.Casting;
        atk_state = DamageManager.AttackStates.Ability1;
        currentAttack = af;
        UpdateAttackInfo();
    }

    public void StartCastingMixup(AttackFrameSO af)
    {
        currentState = States.Casting;
        atk_state = DamageManager.AttackStates.Ability2;
        currentAttack = af;
        UpdateAttackInfo();
    }

    public void EndCasting(float endlag)
    {
        if(currentState != States.HitStun)
        {
            InitiateRecovery(endlag);
        }
    }

    public void ToggleShield(bool on)
    {
        if (on)
        {
            currentState = States.Shield;
        }
        else if(currentState == States.Shield)
        {
            InitiateRecovery(0.25f);
        }
    }

    public bool IsShielding()
    {
        return currentState == States.Shield;
    }

    /*
    public void setStateCasting()
    {
        currentState = States.Casting;
    }
    
    */

    public void setStateIdle()
    {
        currentState = States.Idle;
    }

    public void enterInfiniteRecovery()
    {
        currentState = States.Recovery;
        recoveryTimeLeft = 100000;
    }
    
}
