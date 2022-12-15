using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerController : MonoBehaviour
{
    [SerializeField] Ability ability1;

    //Input Actions
    private PlayerControllerInputAsset input;
    public InputAction playerMove { get; private set; }
    public InputAction playerJump { get; private set; }
    public InputAction abilityOneAction { get; private set; }

    //Standard move values
    private Vector3 StartVelocity = Vector3.zero;
    private Vector2 movementVector;
    [Header("Movement Values")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float movementSmoothing;
    [SerializeField] private float jumpPower;
    [SerializeField] private float doubleJumpPower;
    //Movement bools
    private bool isGrounded;
    private bool canDoubleJump;

    //Grounded things
    [Header("Grounded Check Items")]
    [SerializeField] private Transform groundedCheckObject;
    [SerializeField] private LayerMask groundLayer;

    //Components
    private PlayerManager myPM;
    private PhotonView myPV;
    private Rigidbody2D myRB;

    void Awake()
    {
        myPV = GetComponent<PhotonView>();
        myRB = GetComponent<Rigidbody2D>();

        myPM = PhotonView.Find((int) myPV.InstantiationData[0]).GetComponent<PlayerManager>();

        input = new PlayerControllerInputAsset();
        playerMove = input.Player.Move;
        playerJump = input.Player.Jump;
        abilityOneAction = input.Player.Ability1;

        playerJump.started += jumpBehavior =>
        {
            if (!myPV.IsMine)
            {
                return;
            }
            //If grounded, jump normally
            if (isGrounded)
            {
                myRB.AddForce(new Vector2(myRB.velocity.y, jumpPower * 25));
            }

            //If midair, check if can double jumo
            else if (canDoubleJump)
            {
                canDoubleJump = false;
                myRB.velocity = new Vector2(myRB.velocity.x, 0);
                myRB.AddForce(new Vector2(myRB.velocity.y, doubleJumpPower * 25));
            }
        };

        abilityOneAction.started += ability1Behavior =>
        {
            if (!myPV.IsMine)
            {
                return;
            }

            ability1.Use();
        };
    }

    // Update is called once per frame
    void Update()
    {
        //Return if not local avatar
        if (!myPV.IsMine)
            return;

        //Manage grounded state
        isGrounded = Physics2D.Linecast(transform.position, groundedCheckObject.position, groundLayer);
        if (isGrounded)
        {
            canDoubleJump = true;
        }

        Movement();
    }

    private void Movement()
    {
        //Grounded movement
        movementVector = playerMove.ReadValue<Vector2>();
        if (movementVector.x != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(movementVector.x), transform.localScale.y, transform.localScale.z);
        }
        Vector3 VelocityChange = new Vector2(Time.fixedDeltaTime * moveSpeed * 10 * movementVector.x, myRB.velocity.y);
        myRB.velocity = Vector3.SmoothDamp(myRB.velocity, VelocityChange, ref StartVelocity, movementSmoothing);

    }

    private void OnEnable()
    {
        playerMove.Enable();
        playerJump.Enable();
        abilityOneAction.Enable();
    }

    private void OnDisable()
    {
        playerMove.Disable();
        playerJump.Disable();
        abilityOneAction.Disable();
    }

}
