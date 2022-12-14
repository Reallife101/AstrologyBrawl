using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShotRaycast : Ranged
{
    [SerializeField] Transform raycastOrigin;
    //[SerializeField] LayerMask layerMask;
    [SerializeField] float raycastDistance;

    [SerializeField] float gunDamage;
    public override void Use()
    {
        Shoot();
    }

    void Shoot()
    {
        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin.position, Vector2.right, raycastDistance);
        
        if (hit.collider != null)
        {
            hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(gunDamage);
        }
    }

}
