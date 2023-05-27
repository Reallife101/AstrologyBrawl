using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(PhotonView))]

public class cancerShieldHealth : MonoBehaviourPunCallbacks, IDamageable
{
    [SerializeField]
    private float maxHealth;
    [SerializeField]
    private float lifetime;

    private float currentHealth;
    private PhotonView myPV;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        myPV = GetComponent<PhotonView>();
        StartCoroutine(WaitAndKill(lifetime));
    }

    IEnumerator WaitAndKill(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if (myPV.IsMine)
            PhotonNetwork.Destroy(gameObject);
    }

    public void Die()
    {
        PhotonNetwork.Destroy(gameObject);
    }

    public void TakeDamage(float damage)
    {
        myPV.RPC("RPC_TakeDamage", myPV.Owner, damage, Vector2.zero);
    }

    public void TakeDamage(float damage, Vector2 launchVector, float hitStunValue = 0.25f)
    {
        myPV.RPC("RPC_TakeDamage", myPV.Owner, damage, launchVector, hitStunValue);
    }

    public void healDamage(float heal)
    {
        myPV.RPC("RPC_HealDamage", myPV.Owner, heal);
    }

    [PunRPC]
    public void RPC_HealDamage(float health)
    {
        currentHealth += health;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    [PunRPC]
    public void RPC_TakeDamage(float damage, Vector2 launchVector, float hitStunValue, PhotonMessageInfo info)
    {

        currentHealth -= damage;

        //if health below 0, die
        if (currentHealth <= 0)
        {
            Die();
        }
    }

}
