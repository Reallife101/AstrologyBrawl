using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uiButtons : MonoBehaviour
{
    [SerializeField] FMODUnity.EventReference uiSound;
    public void playSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot(uiSound);
    }
}
