using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class selfProjectile : Ability
{
    [SerializeField] Collider2D hurtbox;
    [SerializeField] float toggleTime;
    [SerializeField] Vector2 forceVector;

    private PhotonView pv;
    private Rigidbody2D rb;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody2D>();
        doDamage dm = GetComponent<doDamage>();
        dm.ownerID = pv.GetInstanceID();
    }
    public override void Use()
    {
        toggleHurtbox();
        rb.AddForce(forceVector, ForceMode2D.Impulse);
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
