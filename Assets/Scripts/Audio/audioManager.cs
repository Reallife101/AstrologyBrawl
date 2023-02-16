using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioManager : MonoBehaviour
{
    [SerializeField] FMODUnity.EventReference footsound;
    [SerializeField] FMODUnity.EventReference lightattacksound;
    [SerializeField] FMODUnity.EventReference jumpsound;
    [SerializeField] float movementspeed;
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

    void Start()
    {
        InvokeRepeating ("CallFootsteps",0,movementspeed);
    }
    
    public void CallLightAttack()
    {
        FMODUnity.RuntimeManager.PlayOneShot(lightattacksound);
    }
    
    public void CallJump()
    {
        FMODUnity.RuntimeManager.PlayOneShot(jumpsound);
    }
}