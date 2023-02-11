using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProjectile : Ranged
{
    [SerializeField] Transform spawnOrigin;
    [SerializeField] GameObject projectile;
    [SerializeField] GameObject player;

    public override void Use()
    {
        spawn();
    }

    void spawn()
    {
        GameObject pro = PhotonNetwork.Instantiate(projectile.name, spawnOrigin.position, Quaternion.identity);

        //If spawned object has doDamage and spawner has a photon view
        PhotonView pv = GetComponent<PhotonView>();
        doDamage dm = pro.GetComponent<doDamage>();
        
        if (pv && dm)
        {
            dm.ownerID = pv.GetInstanceID();
            dm.SetSender(gameObject.transform.parent.gameObject); //SpawnProjectile is always going to be a child of the player prefab
        }

        // If the character faces the other direction, flip the scale and right transform
        if (player.transform.localScale.x<0)
        {
            pro.transform.right = -transform.right.normalized;
            Vector3 proScale = pro.transform.localScale;
            pro.transform.localScale = new Vector3(proScale.x, proScale.y, proScale.z);
        }
    }
}
