using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Health : MonoBehaviourPunCallbacks, IDamageable
{
    [SerializeField] const float MaxHealth = 100f;

    private float currentHealth = MaxHealth;

    private PhotonView myPV;

    private void Awake()
    {
        myPV = GetComponent<PhotonView>();
    }

    public void TakeDamage(float damage)
    {
        myPV.RPC("RPC_TakeDamage", RpcTarget.All, damage);

    }

    [PunRPC]
    void RPC_TakeDamage(float damage)
    {
        if (!myPV.IsMine)
            return;

        currentHealth -= damage;

        if (currentHealth <=0)
        {
            Die();
        }
    }

    void Die()
    {

    }

}
