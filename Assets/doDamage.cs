using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class doDamage : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] bool destroyOnTouch;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!GetComponent<PhotonView>().IsMine)
            return;

        collision.gameObject.gameObject.GetComponent<IDamageable>()?.TakeDamage(damage);
        PhotonNetwork.Destroy(gameObject);
    }
}
