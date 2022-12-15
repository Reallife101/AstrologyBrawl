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
        PhotonNetwork.Instantiate(projectile.name, spawnOrigin.position, Quaternion.identity);
    }
}
