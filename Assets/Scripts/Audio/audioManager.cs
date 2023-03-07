using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

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
    [SerializeField] FMODUnity.EventReference attackvoice;
    [SerializeField] FMODUnity.EventReference chargeattackvoice;
    [SerializeField] FMODUnity.EventReference ability1voice;
    [SerializeField] FMODUnity.EventReference iconicvoice;

    [Header("Misc")]
    [SerializeField] FMODUnity.EventReference genericdeathsound;
    [SerializeField] FMODUnity.EventReference takedamagesound;
    [SerializeField] FMODUnity.EventReference takedamagevoice;
    [SerializeField] FMODUnity.EventReference characterselect;
    [SerializeField] FMODUnity.EventReference spawnvoice;
    
    
    // Other variables
    playerController playerController;

    void Awake()
    {
        playerController = GetComponent<playerController>();
        view.RPC("CallFootsteps", RpcTarget.All);
    }
    
    [PunRPC]
    public void CallFootsteps()
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
        FMODUnity.RuntimeManager.PlayOneShot(attackvoice);
    }

    public void CallHeavyAttackCharge()
    {
        FMODUnity.RuntimeManager.PlayOneShot(heavyattackchargesound);
    }

    public void CallHeavyAttackRelease()
    {
        FMODUnity.RuntimeManager.PlayOneShot(heavyattackreleasesound);
        FMODUnity.RuntimeManager.PlayOneShot(chargeattackvoice);
    }

        public void CallAbility1()
    {
        FMODUnity.RuntimeManager.PlayOneShot(ability1sound);
        FMODUnity.RuntimeManager.PlayOneShot(ability1voice);
    }

        public void CallAbility2()
    {
        FMODUnity.RuntimeManager.PlayOneShot(ability2sound);
        FMODUnity.RuntimeManager.PlayOneShot(iconicvoice);
    }

         public void CallDeathGeneric()
    {
        FMODUnity.RuntimeManager.PlayOneShot(genericdeathsound);
    }   

         public void CallTakeDamage()
    {
        FMODUnity.RuntimeManager.PlayOneShot(takedamagesound);
        FMODUnity.RuntimeManager.PlayOneShot(takedamagevoice);
    }   

         public void CallCharacterSelect()
    {
        FMODUnity.RuntimeManager.PlayOneShot(characterselect);
    }   

         public void CallSpawnVoice()
    {
        FMODUnity.RuntimeManager.PlayOneShot(spawnvoice);
    }   


    void Start()
    {
        InvokeRepeating ("CallFootsteps",0,movementspeed);
    }
}