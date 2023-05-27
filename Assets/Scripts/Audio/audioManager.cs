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
    [SerializeField] FMODUnity.EventReference spawnvoice;
    
    [Header("Capricorn Iconic")]
    [SerializeField] FMODUnity.EventReference damageSound;
    [SerializeField] FMODUnity.EventReference healSound;
    [SerializeField] FMODUnity.EventReference jumpSound;
    [SerializeField] FMODUnity.EventReference speedSound;
    
    // Other variables
    playerController playerController;
    PhotonView photonView;

    void Awake()
    {
        playerController = GetComponent<playerController>();
        photonView = GetComponent<PhotonView>();
        photonView.RPC("Start", RpcTarget.All);
        photonView.RPC("Footsteps", RpcTarget.All);
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

    public void CallAbility1()
    {
        photonView.RPC("Ability1", RpcTarget.All);
    }

    public void CallAbility2()
    {
        photonView.RPC("Ability2", RpcTarget.All);
    }

    public void CallDeathGeneric()
    {
        photonView.RPC("DeathGeneric", RpcTarget.All);
    }

    public void CallTakeDamage()
    {
        photonView.RPC("TakeDamage", RpcTarget.All);
    }

    public void CallSpawnVoice()
    {
        photonView.RPC("SpawnVoice", RpcTarget.All);
    }

    public void CallCapDamage()
    {
        photonView.RPC("CapDamage", RpcTarget.All);
    }

    public void CallCapHeal()
    {
        photonView.RPC("CapHeal", RpcTarget.All);
    }

    public void CallCapJump()
    {
        photonView.RPC("CapJump", RpcTarget.All);
    }

    public void CallCapSpeed()
    {
        photonView.RPC("CapSpeed", RpcTarget.All);
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

    [PunRPC]
    public void Ability1()
    {
        FMODUnity.RuntimeManager.PlayOneShot(ability1sound);
        FMODUnity.RuntimeManager.PlayOneShot(ability1voice);
    }
    
    [PunRPC]
    public void Ability2()
    {
        FMODUnity.RuntimeManager.PlayOneShot(ability2sound);
        FMODUnity.RuntimeManager.PlayOneShot(iconicvoice);
    }

    [PunRPC]
    public void DeathGeneric()
    {
        FMODUnity.RuntimeManager.PlayOneShot(genericdeathsound);
    }   

    [PunRPC]
    public void TakeDamage()
    {
        FMODUnity.RuntimeManager.PlayOneShot(takedamagesound);
        FMODUnity.RuntimeManager.PlayOneShot(takedamagevoice);
    }   

    [PunRPC]
    public void SpawnVoice()
    {
        FMODUnity.RuntimeManager.PlayOneShot(spawnvoice);
    }   


    [PunRPC]
    public void CapDamage()
    {
        FMODUnity.RuntimeManager.PlayOneShot(damageSound);
    }

    [PunRPC]
    public void CapHeal()
    {
        FMODUnity.RuntimeManager.PlayOneShot(healSound);
    }

    [PunRPC]
    public void CapJump()
    {
        FMODUnity.RuntimeManager.PlayOneShot(jumpSound);
    }

    [PunRPC]
    public void CapSpeed()
    {
        FMODUnity.RuntimeManager.PlayOneShot(speedSound);
    }

    [PunRPC]
    public void Start()
    {
        InvokeRepeating ("CallFootsteps",0,movementspeed);
    }

}