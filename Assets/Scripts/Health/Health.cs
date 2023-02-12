using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public abstract class Health : MonoBehaviourPunCallbacks, IDamageable, IPunInstantiateMagicCallback
{
    [SerializeField] const float MaxHealth = 100f;
    [SerializeField] counter cntr;

    public float currentHealth = MaxHealth;
    private bool counter = false;

    public PhotonView myPV;

    private Player lastPlayer;

    public HealthItem healthItem;

    private void Awake()
    {
        myPV = GetComponent<PhotonView>();
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        Debug.Log("IS IT HERE?");

        PlayerManager manager = null;
        Player _player = null;

        if (myPV.IsMine)
        {
            foreach(Player player in PhotonNetwork.PlayerList)
            {
                if(player.ActorNumber == myPV.ViewID / PhotonNetwork.MAX_VIEW_IDS)
                {
                    Debug.Log(player);
                    manager = PlayerManager.Find(player);
                    break;
                }    
            }
        }


        if (manager != null)
        {
            manager.SetController(gameObject);
            manager.UpdateHealth();
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

    [PunRPC]
    public void RPC_TakeDamage(float damage, Vector2 launchVector, float hitStunValue, PhotonMessageInfo info)
    {
        //Check if we countered
        if (counter && cntr != null)
        {
            cntr.onCounter();
            counter = false;
            return;
        }
        
        myPV.RPC("HitStunned", myPV.Owner, hitStunValue);
        currentHealth -= damage;

        healthItem.SetHealthUI(currentHealth);

        GetComponent<Rigidbody2D>().AddForce(launchVector, ForceMode2D.Impulse);

        //if health below 0, die
        if (currentHealth <=0)
        {
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

    public bool getCounter()
    {
        return counter;
    }

    public void setCounter(bool b)
    {
        counter = b;
    }

}
