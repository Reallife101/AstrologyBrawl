using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour

{
    [FMODUnity.EventRef]
    playerController playerController;
    public float movementspeed;
    //public string inputsound;
    public playerController player;

    void Awake()
    {
        playerController = GetComponent<playerController>();
    }
    
    
    void CallFootsteps()
    {
        if (playerController.movementVector.x != 0)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Walkies");
            
        }
            
    }  

    void Start()
    {
        InvokeRepeating ("CallFootsteps",0,movementspeed);
    }

}
