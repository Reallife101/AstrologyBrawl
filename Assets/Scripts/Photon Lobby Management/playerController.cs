using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using Photon.Pun;
using Photon.Realtime;

public class playerController : MonoBehaviour
{
    [SerializeField] Ability ability1;
    [SerializeField] Ability ability2;

    //Input Actions
    private PlayerControllerInputAsset input;
    public InputAction playerMove { get; private set; }
    public InputAction playerJump { get; private set; }
    public InputAction abilityOneAction { get; private set; }
    public InputAction abilityTwoAction { get; private set; }
    public InputAction lightAttackAction { get; private set; }
    public InputAction heavyAttackAction { get; private set; }
    public InputAction scoreboardInputAction { get; private set; }


    //Standard move values
    private Vector3 StartVelocity = Vector3.zero;
    public Vector2 movementVector;
    [Header("Movement Values")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float movementSmoothing;
    [SerializeField] private float jumpPower;
    [SerializeField] private float fastFallSpeed;
    [SerializeField] private float doubleJumpPower;

    //Public setters for move values
    public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }
    public float MovementSmoothing { get { return movementSmoothing; } set { movementSmoothing = value; } }
    public float JumpPower { get { return jumpPower; } set { jumpPower = value; } }
    public float FastFallSpeedg { get { return fastFallSpeed; } set { fastFallSpeed = value; } }
    public float DoubleJumpPower { get { return doubleJumpPower; } set { doubleJumpPower = value; } }
    public float AbilityOneCurrCooldown { get { return ability1.CurrCooldownTime(); } }
    public float AbilityOneMaxCooldown { get { return ability1.MaxCooldownTime(); } }
    public float AbilityTwoCurrCooldown { get { return ability2.CurrCooldownTime(); } }
    public float AbilityTwoMaxCooldown { get { return ability2.MaxCooldownTime(); } }

    //Movement bools
    public bool isGrounded;
    private bool canDoubleJump;
    private bool movementLocked = false;
    private bool fastFall;

    //Grounded things
    [Header("Grounded Check Items")]
    [SerializeField] private Transform groundedCheckObjectLeft;
    [SerializeField] private Transform groundedCheckObjectRight;
    [SerializeField] private LayerMask groundLayer;

    //Audio
    [Header("Audio Scripts")]
    audioManager audioManager;
    private bool fastFallAudioBool;

    //Components
    private PlayerManager myPM;
    private PhotonView myPV;
    private Rigidbody2D myRB;
    private StateManager mySM;
    private Scoreboard myScoreboard;
    private PlayerHealth myHealth;

    //Death Platform
    [SerializeField] private Transform deathPlatformOrigin;
    [SerializeField] private GameObject deathPlatformPrefab;
    private GameObject deathPlatformObject;
    private Coroutine deathPlatformCoroutine;
    [SerializeField] private float respawnInvulTime = 5;

    //Grounded things
    [Header("Animations")]
    [SerializeField] Animator anim;

    //Crown
    [Header("Crown")]
    [SerializeField] private GameObject crown;

    void Awake()
    {
        audioManager = GetComponent<audioManager>();
        fastFallAudioBool = true;
        myPV = GetComponent<PhotonView>();
        myRB = GetComponent<Rigidbody2D>();
        mySM = GetComponent<StateManager>();
        myHealth = GetComponent<PlayerHealth>();
        myScoreboard = FindObjectOfType<Scoreboard>();

        myPM = PhotonView.Find((int) myPV.InstantiationData[0]).GetComponent<PlayerManager>();

        input = new PlayerControllerInputAsset();
        playerMove = input.Player.Move;
        playerJump = input.Player.Jump;
        abilityOneAction = input.Player.Ability1;
        abilityTwoAction = input.Player.Ability2;
        lightAttackAction = input.Player.LightAttack;
        heavyAttackAction = input.Player.HeavyAttack;
        scoreboardInputAction = input.Player.Scoreboard;

        playerJump.started += jumpBehavior =>
        {
            if (!myPV.IsMine || movementLocked)
            {
                return;
            }

            DestroyDeathPlatform();

            //If grounded, jump normally
            if (isGrounded)
            {
                audioManager.CallJump();
                myRB.AddForce(new Vector2(myRB.velocity.y, jumpPower * 25)); 

                if(anim)
                {
                    anim.SetTrigger("jump");
                }
            }

            //If midair, check if can double jumo
            else if (canDoubleJump)
            {
                canDoubleJump = false;
                myRB.velocity = new Vector2(myRB.velocity.x, 0);
                audioManager.CallJump();
                myRB.AddForce(new Vector2(myRB.velocity.y, doubleJumpPower * 25));
                if (anim)
                {
                    anim.SetTrigger("jump");
                }
            }
        };

        abilityOneAction.started += ability1Behavior =>
        {
            if (!myPV.IsMine || movementLocked)
            {
                return;
            }
            DestroyDeathPlatform();

            fastFall = false;
            ability1.activate();
            if (ability1.abilitySoundCheck)
            {
                audioManager.CallAbility1();
            }   
        };

        abilityTwoAction.started += ability2Behavior =>
        {
            if (!myPV.IsMine || movementLocked)
            {
                return;
            }
            DestroyDeathPlatform();

            fastFall = false;
            ability2.activate();
            if (ability2.abilitySoundCheck)
            {
                audioManager.CallAbility2();
            }   
        };

        lightAttackAction.started += lightAttackBehavior =>
        {
            if (!myPV.IsMine)
            {
                return;
            }
            DestroyDeathPlatform();

            fastFall = false;
            mySM.LightAttackPressed(isGrounded);
        };

        heavyAttackAction.performed += heavyAttackBehavior =>
        {
            if (!myPV.IsMine)
            {
                return;
            }
            DestroyDeathPlatform();

            fastFall = false;
            mySM.HeavyAttackPressed(isGrounded);
        };

        heavyAttackAction.canceled += heavyAttackRelease =>
        {
            if (!myPV.IsMine)
            {
                return;
            }

            mySM.EndCharge();
        };



        scoreboardInputAction.started += scoreboardToggle =>
        {
            if (!myPV.IsMine)
            {
                return;
            }

            myScoreboard.ToggleScoreboard();
        };
    }

    // Update is called once per frame
    void Update()
    {
        //Return if not local avatar
        if (!myPV.IsMine)
            return;

        //Manage grounded state
        bool isGroundedLeft = Physics2D.Linecast(transform.position, groundedCheckObjectLeft.position, groundLayer);
        bool isGroundedRight = Physics2D.Linecast(transform.position, groundedCheckObjectRight.position, groundLayer);

        isGrounded = isGroundedLeft || isGroundedRight;
        if (anim)
        {
            anim.SetBool("isGrounded", isGrounded);
        }



        if (isGrounded)
        {
            canDoubleJump = true;
            fastFall = false;
            fastFallAudioBool = true;
        }

        movementLocked = mySM.currentState != StateManager.States.Idle && mySM.currentState != StateManager.States.Recovery;

        Movement();
    }

    public void Movement()
    {
        //If attacking, lock movement
        if (movementLocked)
        {
            return;
        }
        //Grounded movement
        movementVector = playerMove.ReadValue<Vector2>();
        
        if (anim != null)
        {
            anim.SetFloat("speed", Mathf.Abs(movementVector.x));
        }

        //Check for any respawn movement
        if (movementVector != Vector2.zero)
        {
            DestroyDeathPlatform();
        }

        if (movementVector.x != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(movementVector.x), transform.localScale.y, transform.localScale.z);
        }

        // Check for fastFall
        if (movementVector.y<-0.95f && isGrounded == false)
        {
            fastFall = true;
        }

        float ySpeed;
        if (fastFall)
        {
            ySpeed = -fastFallSpeed;
            if (fastFallAudioBool)
            {
                audioManager.CallFastFall();
                fastFallAudioBool = false;
            }
        }
        else
        {
            ySpeed = myRB.velocity.y;
        }

        Vector3 VelocityChange = new Vector2(Time.fixedDeltaTime * moveSpeed * 10 * movementVector.x, ySpeed);
        myRB.velocity = Vector3.SmoothDamp(myRB.velocity, VelocityChange, ref StartVelocity, movementSmoothing);

    }

    public void SpawnDeathPlatform()
    {
        deathPlatformObject = PhotonNetwork.Instantiate(deathPlatformPrefab.name, deathPlatformOrigin.position, Quaternion.identity, 0, new object[] { myPV.ViewID });
        Debug.Log("Did you even spawn bro");
        myPV.RPC(nameof(HealthOffRPC), RpcTarget.All);
        deathPlatformCoroutine = StartCoroutine(DeathPlatformCountdown());
    }

    [PunRPC]
    void HealthOnRPC()
    {
        myHealth.setInvincible(false);
        Debug.Log("Invincible status:" + myHealth.getInvincible());
    }

    [PunRPC]
    void HealthOffRPC()
    {
        myHealth.setInvincible(true);
        Debug.Log("Invincible status:" + myHealth.getInvincible());
    }

    private void DestroyDeathPlatform()
    {
        if (deathPlatformCoroutine != null)
        {
            StopCoroutine(deathPlatformCoroutine);
            deathPlatformCoroutine = null;
        }
        if (deathPlatformObject)
        {
            PhotonNetwork.Destroy(deathPlatformObject);
            deathPlatformObject = null;
            myPV.RPC(nameof(HealthOnRPC), RpcTarget.All);
        }
    }

    private IEnumerator DeathPlatformCountdown()
    {
        yield return new WaitForSeconds(respawnInvulTime);
        DestroyDeathPlatform();
    }

    public void DisableInput()
    {
        input.Player.Disable();
    }

    public void ToggleCrown(bool isOn)
    {
        crown?.SetActive(isOn);
    }

    private void OnEnable()
    {
        playerMove.Enable();
        playerJump.Enable();
        abilityOneAction.Enable();
        abilityTwoAction.Enable();
        lightAttackAction.Enable();
        heavyAttackAction.Enable();
        scoreboardInputAction.Enable();
    }

    private void OnDisable()
    {
        playerMove.Disable();
        playerJump.Disable();
        abilityOneAction.Disable();
        abilityTwoAction.Disable();
        lightAttackAction.Disable();
        heavyAttackAction.Disable();
        scoreboardInputAction.Disable();
    }



}
