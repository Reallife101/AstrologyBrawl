using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class selfProjectile : Ability
{
    [SerializeField] Collider2D hurtbox;
    [SerializeField] float toggleTime;
    [SerializeField] Vector2 forceVector;
    [SerializeField] doDamage dm;

    private PhotonView pv;
    private Rigidbody2D rb;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody2D>();
        dm.ownerID = pv.GetInstanceID();
        dm.SetSender(gameObject);
    }
    public override void Use()
    {
        toggleHurtbox();
        if (transform.localScale.x <=0)
        {
            rb.AddForce(new Vector2(-forceVector.x, forceVector.y), ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(forceVector, ForceMode2D.Impulse);
        }
    }

    void toggleHurtbox()
    {
        if (hurtbox && pv.IsMine)
        {
            hurtbox.enabled = true;
            StartCoroutine(WaitAndKill(toggleTime));
        }
    }

    IEnumerator WaitAndKill(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if (pv.IsMine)
            hurtbox.enabled = false;
    }
}
