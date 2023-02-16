using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpAudio : MonoBehaviour
{
    [SerializeField] FMODUnity.EventReference inputsound;
    
    
    public void CallJump()
    {
        FMODUnity.RuntimeManager.PlayOneShot(inputsound);
    }
}