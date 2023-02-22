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
    [SerializeField] FMODUnity.EventReference ability1sound;
    [SerializeField] FMODUnity.EventReference ability2sound;

    [Header("Misc")]
    [SerializeField] FMODUnity.EventReference genericdeathsound;
    [SerializeField] FMODUnity.EventReference takedamagesound;
    
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

        public void CallAbility1()
    {
        FMODUnity.RuntimeManager.PlayOneShot(ability1sound);
    }

        public void CallAbility2()
    {
        FMODUnity.RuntimeManager.PlayOneShot(ability2sound);
    }

         public void CallDeathGeneric()
    {
        FMODUnity.RuntimeManager.PlayOneShot(genericdeathsound);
    }   

         public void CallTakeDamage()
    {
        FMODUnity.RuntimeManager.PlayOneShot(takedamagesound);
    }   

    void Start()
    {
        InvokeRepeating ("CallFootsteps",0,movementspeed);
    }
}