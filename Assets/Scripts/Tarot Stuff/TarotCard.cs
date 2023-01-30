using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public abstract class TarotCard : MonoBehaviour
{
    protected PhotonView PV;

    protected void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    public abstract void Effect(Player player);
}
