using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour

{
    public FMODUnity.EventReference inputsound;
    public float movementspeed;
    public playerController player;
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
                FMODUnity.RuntimeManager.PlayOneShot(inputsound);
                
            }
        }      
    }  

    void Start()
    {
        InvokeRepeating ("CallFootsteps",0,movementspeed);
    }

}
