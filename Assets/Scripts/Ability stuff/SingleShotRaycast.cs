using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShotRaycast : Ranged
{
    [SerializeField] Transform raycastOrigin;
    //[SerializeField] LayerMask layerMask;
    [SerializeField] float raycastDistance;

    [SerializeField] float gunDamage;
    [SerializeField] float launchForce;
    public override void Use()
    {
        Shoot();
    }

    void Shoot()
    {
        RaycastHit2D hit;
        if (transform.localScale.x <=0)
        {
            hit = Physics2D.Raycast(raycastOrigin.position, -Vector2.right, raycastDistance);
        }
        else
        {
            hit = Physics2D.Raycast(raycastOrigin.position, Vector2.right, raycastDistance);
        }
        
        if (hit.collider != null)
        {
            Vector2 launchVector = new Vector2(hit.transform.position.x - transform.position.x, hit.transform.position.y - transform.position.y + 0.25f);
            hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(gunDamage, launchVector.normalized*launchForce);
        }
    }

}
