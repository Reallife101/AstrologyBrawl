using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public abstract class TarotCard : MonoBehaviour
{
    protected playerController pc;

    private void Awake()
    {
        pc = GetComponent<playerController>();
    }

    public abstract void Effect();
}
