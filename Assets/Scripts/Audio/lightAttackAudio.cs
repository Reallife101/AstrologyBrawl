using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightAttackAudio : MonoBehaviour
{
    [SerializeField] FMODUnity.EventReference inputsound;
    
    
    public void CallLightAttack()
    {
        FMODUnity.RuntimeManager.PlayOneShot(inputsound);
    }
}
