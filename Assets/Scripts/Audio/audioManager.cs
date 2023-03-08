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
    PhotonView photonView;

    void Awake()
    {
        playerController = GetComponent<playerController>();
        photonView = GetComponent<PhotonView>();
        photonView.RPC("Start", RpcTarget.All);
    }
    

    // Sound Calls
    public void CallJump()
    {
        photonView.RPC("Jump", RpcTarget.All);
    }

    public void CallFootsteps()
    {
        photonView.RPC("Footsteps", RpcTarget.All);
    }

    public void CallFastFall()
    {
        photonView.RPC("FastFall", RpcTarget.All);
    }

    public void CallLightAttack()
    {
        photonView.RPC("LightAttack", RpcTarget.All);
    }

    public void CallHeavyAttackCharge()
    {
        photonView.RPC("HeavyAttackCharge", RpcTarget.All);
    }

    public void CallHeavyAttackRelease()
    {
        photonView.RPC("HeavyAttackRelease", RpcTarget.All);
    }

    // Run RPCs
    [PunRPC]
    public void Footsteps()
    {
        if (playerController.isGrounded)
        {
            if (playerController.movementVector.x != 0)
            {
                FMODUnity.RuntimeManager.PlayOneShot(footsound);  
            }
        }      
    }  

    [PunRPC]
    public void Jump()
    {
        FMODUnity.RuntimeManager.PlayOneShot(jumpsound);
    }
    
    [PunRPC]
    public void FastFall()
    {
        FMODUnity.RuntimeManager.PlayOneShot(fastfallsound);
    }

    [PunRPC]
    public void LightAttack()
    {
        FMODUnity.RuntimeManager.PlayOneShot(lightattacksound);
        FMODUnity.RuntimeManager.PlayOneShot(attackvoice);
    }

    [PunRPC]
    public void HeavyAttackCharge()
    {
        FMODUnity.RuntimeManager.PlayOneShot(heavyattackchargesound);
    }

    [PunRPC]
    public void HeavyAttackRelease()
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

    [PunRPC]
    public void Start()
    {
        InvokeRepeating ("CallFootsteps",0,movementspeed);
    }

}