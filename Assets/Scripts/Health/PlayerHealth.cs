using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerHealth : Health
{
    [SerializeField] GameObject healthbar;

    private void Awake()
    {
        healthbar = Instantiate(healthbar);
    }

    public override void Die()
    {
        PhotonView.Find((int)myPV.InstantiationData[0]).GetComponent<PlayerManager>().Die();
    }
}
