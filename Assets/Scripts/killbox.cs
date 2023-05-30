using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;

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
            collision.gameObject.GetComponent<PlayerHealth>().setInvincible(false);
            dmg.TakeDamage(9999999, Vector2.zero, 10);
            PhotonNetwork.Instantiate(deathPrefab.name, collision.transform.position, gameObject.transform.rotation, 0);

            CinemachineShake.Instance.ShakeCamera(10f, .15f);
        }
    }
}
