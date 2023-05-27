using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAudio : MonoBehaviour
{
    [SerializeField] FMODUnity.EventReference UIButton;

    public void callUIAudio()
    {
        FMODUnity.RuntimeManager.PlayOneShot(UIButton);
    }
}
