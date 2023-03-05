using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public abstract class Health : MonoBehaviourPunCallbacks, IDamageable, IPunInstantiateMagicCallback
{
    [SerializeField] const float MaxHealth = 100f;
    [SerializeField] counter cntr;

    public float damageTaken = 0; 
    public float currentHealth = MaxHealth;
    private bool counter = false;
    private bool invincible = false;

    public PhotonView myPV;

    private Player lastPlayer;

    public HealthItem healthItem;

    audioManager audioManager;

    private void Awake()
    {
        myPV = GetComponent<PhotonView>();
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
                Debug.Log(_player);
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
        myPV.RPC("RPC_TakeDamage", myPV.Owner, damage, Vector2.zero);
        audioManager.CallTakeDamage();
    }

    public void TakeDamage(float damage, Vector2 launchVector, float hitStunValue = 0.25f)
    {
        myPV.RPC("RPC_TakeDamage", myPV.Owner, damage, launchVector, hitStunValue);
        audioManager.CallTakeDamage();
    }

    [PunRPC]
    public void RPC_UpdateHealthUI(float damage)
    {
        healthItem.DecreaseHealthUI(damage);
    }

    [PunRPC]
    public void RPC_TakeDamage(float damage, Vector2 launchVector, float hitStunValue, PhotonMessageInfo info)
    {
        //Check if invincible
        if (invincible)
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

        myPV.RPC("RPC_UpdateHealthUI", RpcTarget.All, damage);
        myPV.RPC("HitStunned", myPV.Owner, hitStunValue);
        currentHealth -= damage;

        GetComponent<Rigidbody2D>().AddForce(launchVector, ForceMode2D.Impulse);

        //if health below 0, die
        if (currentHealth <=0)
        {
           audioManager.CallDeathGeneric(); 
            Die();

            //if you are not yourself or nothing, give them a kill
            if (info.Sender != null && info.Sender != PhotonNetwork.LocalPlayer)
            {
                PlayerManager.Find(info.Sender).GetKill();
            }
            //if you have been damaged previously, give them a kill
            else if (lastPlayer != null)
            {
                PlayerManager.Find(lastPlayer).GetKill();
            }
        }

        //Update who damaged you last
        if (info.Sender != PhotonNetwork.LocalPlayer)
        {
            lastPlayer = info.Sender;
        }
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

}
