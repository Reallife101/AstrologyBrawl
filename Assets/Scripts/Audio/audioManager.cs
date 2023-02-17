using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioManager : MonoBehaviour
{
    [Header("Footsteps")]
    [SerializeField] FMODUnity.EventReference footsound;
    [SerializeField] float movementspeed;

    [Header("Movement")]
    [SerializeField] FMODUnity.EventReference jumpsound;
    [SerializeField] FMODUnity.EventReference fastfallsound;

    [Header("Attacks")]
    [SerializeField] FMODUnity.EventReference lightattacksound;
    
    // Other variables
    playerController playerController;
    private float nextFFTime = 10;


    void Awake()
    {
        playerController = GetComponent<playerController>();
    }
    
    void CallFootsteps()
    {
        if (playerController.isGrounded)
        {
            if (playerController.movementVector.x != 0)
            {
                FMODUnity.RuntimeManager.PlayOneShot(footsound);   
            }
        }      
    }  

    public void CallJump()
    {
        FMODUnity.RuntimeManager.PlayOneShot(jumpsound);
    }
    
    public void CallFastFall()
    {
        FMODUnity.RuntimeManager.PlayOneShot(fastfallsound);
    }

    public void CallLightAttack()
    {
        FMODUnity.RuntimeManager.PlayOneShot(lightattacksound);
    }
    
    void Start()
    {
        InvokeRepeating ("CallFootsteps",0,movementspeed);
    }
}