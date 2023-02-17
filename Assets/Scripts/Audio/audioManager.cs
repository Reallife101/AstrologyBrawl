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
    [SerializeField] FMODUnity.EventReference heavyattackchargesound;
    [SerializeField] FMODUnity.EventReference heavyattackreleasesound;
    
    
    // Other variables
    playerController playerController;

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

    public void CallHeavyAttackCharge()
    {
        FMODUnity.RuntimeManager.PlayOneShot(heavyattackchargesound);
    }

    public void CallHeavyAttackRelease()
    {
        FMODUnity.RuntimeManager.PlayOneShot(heavyattackreleasesound);
    }
    
    void Start()
    {
        InvokeRepeating ("CallFootsteps",0,movementspeed);
    }
}