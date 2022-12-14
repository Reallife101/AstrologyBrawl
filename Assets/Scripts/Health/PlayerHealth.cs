using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerHealth : Health
{

    public override void Die()
    {
        PhotonView.Find((int)myPV.InstantiationData[0]).GetComponent<PlayerManager>().Die();
    }

}
