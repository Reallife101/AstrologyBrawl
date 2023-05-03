using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doGeminiDamage : doDamage
{
    [SerializeField] private GameObject otherBro;
    public override bool extraDamageCheck(Collider2D collision)
    {
        return collision.gameObject.GetComponent<PhotonView>()?.GetInstanceID() == otherBro.GetComponent<PhotonView>()?.GetInstanceID();
    }
}
