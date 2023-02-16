using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerHealth : Health
{
    [SerializeField] GameObject deathEffect;
    public override void Die()
    {
        if (deathEffect != null)
        {
            PhotonNetwork.Instantiate(deathEffect.name, gameObject.transform.position, gameObject.transform.rotation, 0);
        }

        PhotonView.Find((int)myPV.InstantiationData[0]).GetComponent<PlayerManager>().Die();
    }
}
