using ExitGames.Client.Photon;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class doDamage : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] bool destroyOnTouch;
    [SerializeField] float lifeTime;
    [SerializeField] bool hasInfiniteLifeTime;
    [SerializeField] float launchForce;
    [SerializeField] float hitStunTime = 0.25f;
    [SerializeField] Vector2 launchDirection;
    [SerializeField] Transform parentSprite;

    private float counterDamage = 0;
    private GameObject AttackSender; 

    public int ownerID;

    private PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        if (!hasInfiniteLifeTime)
        {
            StartCoroutine(WaitAndKill(lifeTime));
        }

    }

    IEnumerator WaitAndKill(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if (pv.IsMine)
            PhotonNetwork.Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!pv || !pv.IsMine || collision.gameObject.GetComponent<PhotonView>()?.GetInstanceID() == ownerID)
            return;

        // Get components
        IDamageable dmg = collision.gameObject.GetComponent<IDamageable>();
       

        if (dmg != null)
        {
            if (AttackSender != null)
            {
                libraCounter counter = AttackSender.GetComponent<libraCounter>();
                Debug.Log(counter);
                if (counter != null)
                {
                    counterDamage = counter.get_DamageIncrease();
                    //Debug.Log("Countered Damage " + counterDamage);
                    damage += counterDamage;
                    //Debug.Log(damage);
                    counter.ResetDamage();
                }
            }
            //Check to see which launch we should use
            if (launchDirection.magnitude >.1)
            {
                
                //flip the x if we are facing the wrong direction
                if (transform.localScale.x < 0 || (parentSprite != null && parentSprite.localScale.x < 0))
                {
                    dmg.TakeDamage(damage, new Vector2(-launchDirection.x, launchDirection.y), hitStunTime);
                }
                else
                {
                    dmg.TakeDamage(damage, launchDirection, hitStunTime);
                }
            }
            else
            {
                Vector2 launchVector = new Vector2(collision.transform.position.x - transform.position.x, collision.transform.position.y - transform.position.y + 0.25f);
                dmg.TakeDamage(damage, launchVector * launchForce, hitStunTime);
            }

            //Destroy object
            if (destroyOnTouch)
            {
                PhotonNetwork.Destroy(gameObject);
            }

            if (counterDamage > 0) { 
                damage -= counterDamage;
                counterDamage = 0;
            }
        }
    }


    public void SetValues(float damageNum)
    {
        damage = damageNum;
    }


    public void SetValues(float damageNum, float hitStunNum, float knockbackNum, Vector2 launchDir, GameObject sender)
    {
        damage = damageNum;
        hitStunTime = hitStunNum;
        launchForce = knockbackNum;
        launchDirection = launchDir;
        AttackSender = sender;
    }

    public void SetSender(GameObject sender) {

        AttackSender = sender;
    
    }



}
