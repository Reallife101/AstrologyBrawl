using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProjectile : Ranged
{
    [SerializeField] Transform spawnOrigin;
    [SerializeField] GameObject projectile;

    public override void Use()
    {
        spawn();
    }

    void spawn()
    {
        GameObject pro = PhotonNetwork.Instantiate(projectile.name, spawnOrigin.position, Quaternion.identity);
        
        // If the character faces the other direction, flip the scale and right transform
        if (transform.localScale.x<0)
        {
            pro.transform.right = -transform.right.normalized;
            Vector3 proScale = pro.transform.localScale;
            pro.transform.localScale = new Vector3(proScale.x, proScale.y, proScale.z);
        }
    }
}
