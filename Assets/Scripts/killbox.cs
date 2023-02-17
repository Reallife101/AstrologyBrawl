using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class killbox : MonoBehaviour
{
    [SerializeField] GameObject deathPrefab;
    private void OnTriggerStay2D(Collider2D collision)
    {

        // Get components
        IDamageable dmg = collision.gameObject.GetComponent<IDamageable>();

        if (dmg != null)
        {
            //kill the player
            dmg.TakeDamage(9999999, Vector2.zero, 10);
            PhotonNetwork.Instantiate(deathPrefab.name, collision.transform.position, gameObject.transform.rotation, 0);
        }
    }
}
