using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(PhotonView))]
public class shieldHealth : MonoBehaviourPunCallbacks, IDamageable
{
    [SerializeField]
    private float shieldDrainPerSecond;
    [SerializeField]
    private float shieldRegenPerSecond;

    private float currentHealth;
    [SerializeField]
    private float maxHealth;

    [SerializeField]
    private SpriteRenderer shieldIcon;
    [SerializeField]
    private float maxSize;
    [SerializeField]
    private PlayerHealth playerHealth;

    private bool isActive;
    private bool isInvincible;
    private bool isRegen;
    private bool canActivate;
    private PhotonView myPV;


    private void Awake()
    {
        isActive = false;
        isRegen = true;
        canActivate = true;
        isInvincible = false;
        currentHealth = maxHealth;
        shieldIcon.enabled = false;
        myPV = GetComponent<PhotonView>();
    }
    public void Die()
    {
        Debug.Log("Shield Broken!");
        isRegen = false;
        shieldIcon.enabled = false;
        playerHealth.setInvincible(false);
        isInvincible = true;
        canActivate = false;
    }

    public void activate()
    {
        isActive = true;
    }

    public void deactivate()
    {
        isActive = false;
    }

    private void Update()
    {
        if (canActivate)
        {
            if (isActive)
            {
                TakeDamage(shieldDrainPerSecond * Time.deltaTime, Vector2.zero, 0);
                playerHealth.setInvincible(true);
                isInvincible = false;
                shieldIcon.enabled = true;

                float size = (currentHealth / maxHealth) * maxSize;

                shieldIcon.transform.localScale = new Vector3(size, size, size);
            }
            else if (currentHealth < maxHealth && isRegen)
            {
                healDamage(shieldRegenPerSecond * Time.deltaTime);
                playerHealth.setInvincible(false);
                shieldIcon.enabled = false;
                isInvincible = true;
            }
        }

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
        //Check if invincible
        if (isInvincible)
        {
            return;
        }

        if (gameObject.tag == "Player")
        {
            myPV.RPC("ZoomCamRPC", RpcTarget.All, damage);
        }

        currentHealth -= damage;

        //if health below 0, die
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    [PunRPC]
    public void ZoomCamRPC(float damage)
    {
        Debug.Log("RPC Called - Zoom");
        ZoomCam.instance.ZoomIn(gameObject, damage);
    }
}
