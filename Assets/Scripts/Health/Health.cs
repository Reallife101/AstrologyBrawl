using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public abstract class Health : MonoBehaviourPunCallbacks, IDamageable
{
    [SerializeField] const float MaxHealth = 100f;

    public float currentHealth = MaxHealth;

    public PhotonView myPV;

    private Player lastPlayer;

    private void Awake()
    {
        myPV = GetComponent<PhotonView>();
    }

    public void TakeDamage(float damage)
    {
        myPV.RPC("RPC_TakeDamage", myPV.Owner, damage, Vector2.zero);

    }

    public void TakeDamage(float damage, Vector2 launchVector, float hitStunValue = 0.25f)
    {
        myPV.RPC("RPC_TakeDamage", myPV.Owner, damage, launchVector, hitStunValue);

    }

    [PunRPC]
    public void RPC_TakeDamage(float damage, Vector2 launchVector, float hitStunValue, PhotonMessageInfo info)
    {
        myPV.RPC("HitStunned", myPV.Owner, hitStunValue);
        currentHealth -= damage;

        GetComponent<Rigidbody2D>().AddForce(launchVector, ForceMode2D.Impulse);

        if (currentHealth <=0)
        {
            Die();
            if (info.Sender != null)
            {
                PlayerManager.Find(info.Sender).GetKill();
            }
            else if (lastPlayer != null)
            {
                PlayerManager.Find(lastPlayer).GetKill();
            }
        }
        lastPlayer = info.Sender;
    }

    public abstract void Die();

}
