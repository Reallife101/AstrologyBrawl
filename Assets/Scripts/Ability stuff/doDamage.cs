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
        if (!pv.IsMine || collision.gameObject.GetComponent<PhotonView>()?.GetInstanceID() == ownerID)
            return;

        // Get components
        IDamageable dmg = collision.gameObject.GetComponent<IDamageable>();
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

        if (dmg != null)
        {
            dmg.TakeDamage(damage);

            //Apply forces
            if (rb)
            {
                Vector2 launchVector = new Vector2(collision.transform.position.x - transform.position.x, collision.transform.position.y - transform.position.y);
                //rb.AddForce(launchVector * 50, ForceMode2D.Impulse);
            }

            //Destroy object
            if (destroyOnTouch)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    public void SetDamage(int damageNum)
    {
        damage = damageNum;
    }

}
