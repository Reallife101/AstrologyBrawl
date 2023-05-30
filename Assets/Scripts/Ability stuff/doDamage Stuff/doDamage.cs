using ExitGames.Client.Photon;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class doDamage : MonoBehaviour
{
    [SerializeField] private DamageManager.AttackStates attack_type;
    [SerializeField] float damage;
    [SerializeField] bool destroyOnTouch;
    [SerializeField] bool destroyOnImpact;
    [SerializeField] float lifeTime;
    [SerializeField] bool hasInfiniteLifeTime;
    [SerializeField] float launchForce;
    [SerializeField] float hitStunTime = 0.25f;
    [SerializeField] Vector2 launchDirection;
    private float shakeTime = 0f;
    private float shakePower = 0f;
    [SerializeField] Transform parentSprite;
    [SerializeField] float baseDamageMultiplier = 1;
   
    private GameObject AttackSender;
    private float multiplier = 1; 

    private DamageManager dmgManager;

    public int ownerID;

    private PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        if (!hasInfiniteLifeTime)
        {
            StartCoroutine(WaitAndKill(lifeTime));
        }

        dmgManager = GetComponent<DamageManager>();

    }

    IEnumerator WaitAndKill(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if (pv.IsMine)
            PhotonNetwork.Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (!pv || !pv.IsMine || collision.gameObject.GetComponent<PhotonView>()?.GetInstanceID() == ownerID || extraDamageCheck(collision))
            return;
        // Get components
        IDamageable dmg = collision.gameObject.GetComponent<IDamageable>();

        
        if (dmg != null)
        {
            float stacked_dmg = 0;

            if (AttackSender != null)
            {
                if (!dmgManager)
                    dmgManager = AttackSender.GetComponent<DamageManager>();

                if (dmgManager && dmgManager.stackedDamage > 0)
                    stacked_dmg = dmgManager.applyStackDamage(); //aplies stacked damage

            }

            //Debug.Log("BEFORE DAMAGE CHANGE " + damage);

            damage = dmgManager.getAttackValue(attack_type) + stacked_dmg;
            float buffMult = dmgManager.getBuffValue(attack_type);

            //Debug.Log("After Damage Change" + damage);
            extraEffect(damage);
            //Check to see which launch we should use
            if (launchDirection.magnitude >.1)
            {
                //flip the x if we are facing the wrong direction
                if (transform.localScale.x < 0 || (parentSprite != null && parentSprite.localScale.x < 0) || (AttackSender != null && AttackSender.transform.localScale.x < 0))
                {
                    dmg.TakeDamage(damage * multiplier * baseDamageMultiplier * buffMult, new Vector2(-launchDirection.x, launchDirection.y), hitStunTime);
                }
                else
                {
                    dmg.TakeDamage(damage * multiplier * baseDamageMultiplier * buffMult, launchDirection, hitStunTime);
                }
            }
            else
            {
                Vector2 launchVector = new Vector2(collision.transform.position.x - transform.position.x, collision.transform.position.y - transform.position.y + 0.25f);
                dmg.TakeDamage(damage * multiplier * baseDamageMultiplier * buffMult, launchVector.normalized * launchForce, hitStunTime);
            }

            if(shakeTime > 0 && shakePower > 0)
            {
                pv.RPC("DamageShake", RpcTarget.All, shakePower, shakeTime);
            }

            AttackSender.GetComponent<playerController>().doDevilDamage(damage * multiplier * baseDamageMultiplier * buffMult);
            //Destroy object
            if (destroyOnTouch)
            {
                PhotonNetwork.Destroy(gameObject);
            }

        }
        else if (destroyOnImpact)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }


    public void SetValues(float damageNum)
    {
        damage = damageNum;
    }

    public void SetValues(float damage, DamageManager.AttackStates type, float chardgeMultiplier, float KBValue, GameObject sender)
    {
        Debug.Log("FROM DO DAMAGE: Damage: " + damage + " Type: " + type);
        sender.GetComponent<DamageManager>().setDamage(type, damage, 0, true);
        dmgManager.setDamage(attack_type, damage, 0, true);
        attack_type = type;
        multiplier = chardgeMultiplier;
        launchForce = KBValue;
    }
    public void SetValues(float damage, DamageManager.AttackStates type, float hitStunNum, float knockbackNum, Vector2 launchDir, float shaketime, float shakepower, GameObject sender, float chargeMulti = 1)
    {
        Debug.Log("FROM DO DAMAGE (LONG FUNCTION): Damage: " + damage + " Type: " + type);
        sender.GetComponent<DamageManager>().setDamage(type, damage, 0, true);
        attack_type = type;
        hitStunTime = hitStunNum;
        launchForce = knockbackNum;
        launchDirection = launchDir;
        AttackSender = sender;
        multiplier = chargeMulti;
        shakePower = shakepower;
        shakeTime = shaketime;
    }

    public void SetSender(GameObject sender) {

        AttackSender = sender;
    
    }

    public virtual void extraEffect(float damage)
    {
        //overide for future use
    }

    public virtual bool extraDamageCheck(Collider2D collision)
    {
        //overide for future use
        return false;
    }

    [PunRPC]
    private void DamageShake(float shakePower, float shakeTime)
    {
        CinemachineShake.Instance.ShakeCamera(shakePower, shakeTime);
    }

}
