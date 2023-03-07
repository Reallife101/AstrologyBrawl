using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountdownAudio : MonoBehaviour
{
    [SerializeField] FMODUnity.EventReference countsound;

    public void CallCountSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot(countsound);
    }
}
