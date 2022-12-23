using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public abstract class Health : MonoBehaviourPunCallbacks, IDamageable
{
    [SerializeField] const float MaxHealth = 100f;

    private float currentHealth = MaxHealth;

    public PhotonView myPV;

    private void Awake()
    {
        myPV = GetComponent<PhotonView>();
    }

    public void TakeDamage(float damage)
    {
        myPV.RPC("RPC_TakeDamage", RpcTarget.All, damage);

    }

    [PunRPC]
    public void RPC_TakeDamage(float damage)
    {
        if (!myPV.IsMine)
            return;

        currentHealth -= damage;

        GetComponent<Rigidbody2D>().AddForce(Vector2.up * 50f, ForceMode2D.Impulse);

        if (currentHealth <=0)
        {
            Die();
        }
    }

    public abstract void Die();

}
