using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(PhotonView))]
public abstract class Health : MonoBehaviourPunCallbacks, IDamageable, IPunInstantiateMagicCallback
{
    [SerializeField] const float MaxHealth = 100f;
    [SerializeField] counter cntr;

    public float damageTaken = 0;
    public float currentHealth = MaxHealth;
    private bool counter = false;
    private bool invincible = false;
    private bool lovers = false;

    public PhotonView myPV;

    private Player lastPlayer;

    public HealthItem healthItem;

    audioManager audioManager;

    private void Awake()
    {
        if (!myPV)
        {
            myPV = GetComponent<PhotonView>();
        }
        audioManager = GetComponent<audioManager>();
    }

    //So that we avoid any null reference exceptions
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        Player player = null;
        PlayerManager manager = null;

        foreach (Player _player in PhotonNetwork.PlayerList)
        {
            if (_player.ActorNumber == myPV.ViewID / PhotonNetwork.MAX_VIEW_IDS)
            {
                player = _player;
                break;
            }
        }

        //HealthItem has no photon view, so we must provide all versions of it the same information through the PlayerManager
        if (player != null)
        {
            manager = PlayerManager.Find(player);
            manager.SetController(gameObject);
            manager.UpdateHealth();
        }
    }

    public void TakeDamage(float damage)
    {
        myPV.RPC("RPC_TakeDamage", myPV.Owner, damage, Vector2.zero, 0f);
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
        if (currentHealth > MaxHealth)
        {
            currentHealth = MaxHealth;
        }
        myPV.RPC("RPC_SetHealthUI", RpcTarget.All, health);
    }

    [PunRPC]
    public void RPC_SetHealthUI(float health)
    {
        if (healthItem)
        {
            healthItem.IncreaseHealthUI(health);
        }
    }

    [PunRPC]
    public void RPC_UpdateHealthUI(float damage)
    {
        if (healthItem)
        {
            healthItem.DecreaseHealthUI(damage);
        }
    }

    [PunRPC]
    public void RPC_TakeDamage(float damage, Vector2 launchVector, float hitStunValue, PhotonMessageInfo info)
    {
        if (damage > 10000)
        {
            lovers = false;
        }
        //Check if invincible
        if (invincible || lovers)
        {
            return;
        }

        //Check if we countered
        if (counter && cntr != null)
        {
            damageTaken = damage;
            cntr.onCounter();

            return;
        }

        if (gameObject.tag == "Player")
        {
            myPV.RPC("ZoomCamRPC", RpcTarget.All, damage);
        }

        myPV.RPC("RPC_UpdateHealthUI", RpcTarget.All, damage);
        myPV.RPC("HitStunned", myPV.Owner, hitStunValue);
        audioManager.CallTakeDamage();
        currentHealth -= damage;

        //apply Force if applicable
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb)
        {
            rb.AddForce(launchVector, ForceMode2D.Impulse);
        }

        //if health below 0, die
        if (currentHealth <= 0)
        {
            audioManager.CallDeathGeneric();
            audioManager.CallSpawnVoice();
            //if you are not yourself or nothing, give them a kill
            if (info.Sender != null && info.Sender != PhotonNetwork.LocalPlayer)
            {
                PlayerManager.Find(info.Sender).GetKill();
                myPV.RPC("MakeKillFeedRPC", RpcTarget.All, info.Sender, PhotonNetwork.LocalPlayer);
            }
            //if you have been damaged previously, give them a kill
            else if (lastPlayer != null)
            {
                PlayerManager.Find(lastPlayer).GetKill();
                myPV.RPC("MakeKillFeedRPC", RpcTarget.All, lastPlayer, PhotonNetwork.LocalPlayer);
            }
            Die();
        }

        //Update who damaged you last
        if (info.Sender != PhotonNetwork.LocalPlayer)
        {
            lastPlayer = info.Sender;
        }
    }

    [PunRPC]
    public void ZoomCamRPC(float damage)
    {
        ZoomCam.instance.ZoomIn(gameObject, damage);
    }

    [PunRPC]
    public void MakeKillFeedRPC(Player Killer, Player KilledPlayer)
    {
        KillFeed.Instance.MakeKillFeed(Killer, KilledPlayer);
    }

    public abstract void Die();

    //make getters and setters
    public float getMaxHealth()
    {
        return MaxHealth;
    }

    public void setHealthItem(HealthItem item)
    {
        healthItem = item;
    }

    public float getCurrentHealth()
    {
        return currentHealth;
    }

    public void setCurrentHealth(float newCurrHealth)
    {
        float difference = currentHealth - newCurrHealth;
        currentHealth = newCurrHealth;
        myPV.RPC("RPC_UpdateHealthUI", RpcTarget.All, difference);
    }

    public bool getCounter()
    {
        return counter;
    }

    public void setCounter(bool b)
    {
        counter = b;
    }

    public bool getInvincible()
    {
        return invincible;
    }

    public void setInvincible(bool b)
    {
        invincible = b;
    }

    public void setLoversInvincible(bool b)
    {
        lovers = b;
    }

}
